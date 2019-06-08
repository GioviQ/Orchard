using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;

namespace CustomCK.Models
{
	public class CustomCKSettingsPartRecord : ContentPartRecord
	{

		public const string DefaultConfigString = @"toolbarGroups: [
	{ name: 'styles' },
	{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
	{ name: 'tools' },
	{ name: 'editing', groups: ['find', 'selection', 'spellchecker'] },
	{ name: 'insert' },
	{ name: 'links' },
	{ name: 'forms' },
	{ name: 'document', groups: ['mode', 'document', 'doctools'] },
	{ name: 'others' },
	{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align'] },
	{ name: 'colors' },
	{ name: 'clipboard', groups: ['clipboard', 'undo'] },
	{ name: 'about' },
],
removeButtons: 'Underline,Subscript,Superscript',
removeDialogTabs: 'image:advanced;link:advanced',
language: 'de',
entities: false,
forcePasteAsPlainText: true,
removePlugins: 'specialchar',
scayt_autoStartup: true,
scayt_sLang: 'de_DE',
codemirror: {
	useBeautify: true 
},
format_tags: 'p;h1;h2;h3;h4',
extraPlugins: 'medialibrary'";

		private string _configString = DefaultConfigString;

		[StringLengthMax]
		public virtual string ConfigString
		{
			get { return _configString; }
			set { _configString = value; }
		}
	}
}
