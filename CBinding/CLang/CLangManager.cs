using System;
using CBinding;
using ClangSharp;
using System.Collections.Generic;
using System.Security.Cryptography;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace CBinding
{
	public class CLangManager
	{
		private static CLangManager instance;
		public static CLangManager Instance
		{
			get {
				if (instance == null)
					instance = new CLangManager ();

				return instance;
			}
		}
		private CXIndex index;
		private Dictionary<string, CXTranslationUnit> translationUnits;
		private CXUnsavedFile[] UnsavedFiles
		{
			get {
				List<CXUnsavedFile> unsavedFiles = new List<CXUnsavedFile>();
				foreach (Document doc in MonoDevelop.Ide.IdeApp.Workbench.Documents) {
					if (doc.IsDirty)
					{CXUnsavedFile unsavedFile = new CXUnsavedFile();
						unsavedFile.Length = doc.Editor.Text.Length;
						unsavedFile.Contents = doc.Editor.Text;
						unsavedFiles.Add (unsavedFile);
					}
				}
				return unsavedFiles.ToArray ();
			}
		}

		private CLangManager ()
		{
			index = clang.createIndex (0, 0);
			translationUnits = new Dictionary<string, CXTranslationUnit> ();
		}
		~CLangManager ()
		{
			foreach (CXTranslationUnit unit in translationUnits.Values)
				clang.disposeTranslationUnit (unit);
			clang.disposeIndex (index);
		}
		public void AddToTranslationUnits(CProject project, string fileName)
		{
			CProjectConfiguration active_configuration 
			= project.DefaultConfiguration as CProjectConfiguration;
			translationUnits.Add (fileName, 
				clang.createTranslationUnitFromSourceFile (
					index,
					fileName,
					0,
					null,
					Convert.ToUInt32(UnsavedFiles.Length),
					UnsavedFiles
				)
			);
		}

		public void UpdateTranslationUnit(CProject project, string fileName)
		{	
			CProjectConfiguration active_configuration 
			= project.DefaultConfiguration as CProjectConfiguration;
			clang.disposeTranslationUnit (translationUnits [fileName]);
			translationUnits [fileName] = clang.createTranslationUnitFromSourceFile (
				index,
				fileName,
				0,
				null,
				Convert.ToUInt32(UnsavedFiles.Length),
				UnsavedFiles
			);
		}

		public void RemoveTranslationUnit(CProject project, string fileName)
		{
			clang.disposeTranslationUnit (translationUnits [fileName]);
			translationUnits.Remove (fileName);
		}
	}
}

