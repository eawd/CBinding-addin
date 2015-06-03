using System;
using ClangSharp;

namespace CBinding
{
	public class CLangManager
	{
		CXIndex index;
		CXTranslationUnit[] translationUnits;
		public CLangManager ()
		{
			index = clang.createIndex (0, 0);
			translationUnits = new CXTranslationUnit[0];
			int i = 0;
			//foreach (file in projectfiles)
			//	translationUnits [i] = clang.createTranslationUnitFromSourceFile (index, file.FileName, );
		}
	}
}

