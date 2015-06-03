
using System;
using Mono.Addins;
using Mono.Addins.Description;

[assembly:Addin ("CBinding", 
	Namespace = "MonoDevelop",
	Version = MonoDevelop.BuildInfo.Version,
	Category = "Language bindings")]

[assembly:AddinName ("C/C++ Language Binding")]
[assembly:AddinDescription ("C/C++ Language binding")]

/* TODO: this should be grabbed from the current buildinfo object, and going to be so,
   but to provide working functionality with WIP 6.0 MD i hardcoded these strings*/
[assembly:AddinDependency ("Core", "6.0")]
[assembly:AddinDependency ("Ide", "6.0")]
[assembly:AddinDependency ("DesignerSupport", MonoDevelop.BuildInfo.Version)]
[assembly:AddinDependency ("Deployment", MonoDevelop.BuildInfo.Version)]
[assembly:AddinDependency ("Deployment.Linux", MonoDevelop.BuildInfo.Version)]
[assembly:AddinDependency ("Refactoring", MonoDevelop.BuildInfo.Version)]
[assembly:AddinDependency ("SourceEditor2", MonoDevelop.BuildInfo.Version)]
