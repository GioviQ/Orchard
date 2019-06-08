using Orchard.UI.Resources;

namespace CustomCK {
	public class ResourceManifest : IResourceManifestProvider{
		public void BuildManifests(ResourceManifestBuilder builder){
			builder.Add().DefineScript("CKEditor").SetUrl("ckeditor/ckeditor.js");
            builder.Add().DefineScript("CodeMirror").SetUrl("codemirror/codemirror-compressed.js");
            builder.Add().DefineStyle("CodeMirror").SetUrl("codemirror/codemirror.css");
		}
	}
}
