using System;
using CBinding;
using ClangSharp;
using System.Collections.Generic;
using System.Security.Cryptography;

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
		CXIndex index;
		Dictionary<string, CXTranslationUnit> translationUnits;
		Dictionary<string, CXUnsavedFile> unsavedFiles;

		private CLangManager ()
		{
			index = clang.createIndex (0, 0);
			translationUnits = new Dictionary<string, CXTranslationUnit> ();
			unsavedFiles = new Dictionary<string, CXUnsavedFile> ();

		}

		public void AddToTranslationUnits(CProject project, string fileName){
			CXUnsavedFile[] unsavedFilesArray = new CXUnsavedFile[unsavedFiles.Count];
			unsavedFiles.Values.CopyTo (unsavedFilesArray,0);
			translationUnits.Add (fileName, 
				clang.createTranslationUnitFromSourceFile (
					index,
					fileName,
					0,
					null,
					Convert.ToUInt32(unsavedFiles.Count),
					unsavedFilesArray
				)
			);
		}

		public void UpdateTranslationUnit(CProject project, string fileName){
			CXUnsavedFile[] unsavedFilesArray = new CXUnsavedFile[unsavedFiles.Count];
			unsavedFiles.Values.CopyTo (unsavedFilesArray,0);
			translationUnits [fileName] = clang.createTranslationUnitFromSourceFile (
				index,
				fileName,
				0,
				null,
				Convert.ToUInt32(unsavedFiles.Count),
				unsavedFilesArray
			);
		}

		public void RemoveTranslationUnit(CProject project, string fileName){
			translationUnits.Remove (fileName);
		}
	}
}

