using System;
using CBinding;
using ClangSharp;
using System.Collections.Generic;
using System.Security.Cryptography;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using System.Text;
using MonoDevelop.Core;
using System.IO;
using MonoDevelop.Projects;

namespace CBinding
{
	public class CLangManager
	{
		private static CLangManager instance;

		public static CLangManager Instance {
			get {
				if (instance == null)
					instance = new CLangManager ();

				return instance;
			}
		}

		private CXIndex index;
		private Dictionary<string, CXTranslationUnit> translationUnits;

		private CXUnsavedFile[] UnsavedFiles {
			get {
				List<CXUnsavedFile> unsavedFiles = new List<CXUnsavedFile> ();
				foreach (Document doc in MonoDevelop.Ide.IdeApp.Workbench.Documents) {
					if (doc.IsDirty) {
						CXUnsavedFile unsavedFile = new CXUnsavedFile ();
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

		public void AddToTranslationUnits (CProject project, string fileName)
		{
			ClangCCompiler compiler = new ClangCCompiler ();
			CProjectConfiguration active_configuration =
				(CProjectConfiguration)project.GetConfiguration (IdeApp.Workspace.ActiveConfiguration);

			string[] args = compiler.GetCompilerFlagsAsArray (project, active_configuration);
			translationUnits.Add (fileName, 
				clang.createTranslationUnitFromSourceFile (
					index,
					fileName,
					args.Length,
					args,
					Convert.ToUInt32 (UnsavedFiles.Length),
					UnsavedFiles
				)
			);
		}

		public void UpdateTranslationUnit (CProject project, string fileName)
		{
			ClangCCompiler compiler = new ClangCCompiler ();
			CProjectConfiguration active_configuration =
				(CProjectConfiguration)project.GetConfiguration (IdeApp.Workspace.ActiveConfiguration);
			string[] args = compiler.GetCompilerFlagsAsArray (project, active_configuration);
			clang.disposeTranslationUnit (translationUnits [fileName]);
			translationUnits [fileName] = clang.createTranslationUnitFromSourceFile (
				index,
				fileName,
				args.Length,
				args,
				Convert.ToUInt32 (UnsavedFiles.Length),
				UnsavedFiles
			);
		}

		public void RemoveTranslationUnit (CProject project, string fileName)
		{
			clang.disposeTranslationUnit (translationUnits [fileName]);
			translationUnits.Remove (fileName);
		}
	}
}

