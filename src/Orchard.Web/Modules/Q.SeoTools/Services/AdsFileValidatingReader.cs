using Orchard.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Q.SeoTools.Services
{

    public class AdsFileValidatingReader : IDisposable
    {
        private const string SyntaxNotUnderstoodError = "Syntax not understood";

        private List<AdsFileValidationError> _errors;
        private StringReader _reader;
        private int _currentLineNumber;

        public AdsFileValidatingReader(string robotsFileText)
        {
            _errors = new List<AdsFileValidationError>();
            _reader = new StringReader(robotsFileText);
            _currentLineNumber = 0;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public bool ValidateNext()
        {
            string line = ReadLine();
            if (line == null)
                return false;
            Validate(line);
            _currentLineNumber++;
            return true;
        }

        public void ValidateToEnd()
        {
            string line = ReadLine();
            while (line != null)
            {
                Validate(line);
                line = ReadLine();
            }
        }

        private string ReadLine()
        {
            string line = null;
            try
            {
                line = _reader.ReadLine();
                return line;
            }
            finally
            {
                if (line != null)
                    _currentLineNumber++;
            }
        }

        private void Validate(string line)
        {
            // Allow blank lines
            if (string.IsNullOrWhiteSpace(line))
            {
                // If the line is blank, then that means we should get a new user agent directive on the following line or EOF
                
                return;
            }
            // Allow comments, ads.txt comments start with # (pound), in this case, the entire line is ignored
            if (line.StartsWith("#"))
                return;

            // Will match any valid line in a good ads.txt file (assumes single line)
            Regex syntaxMatcher = new Regex(@"^(?<Domain>([a-zA-Z0-9][a-zA-Z0-9-]{1,61}[a-zA-Z0-9]\.[a-zA-Z]{2,}),(?<Publisher>.+),(?<Type>(DIRECT|RESELLER)(,(?<AuthorityID>.+))?)$", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
            // Will match any comments in-line with the current line (valid if not a full line of comments)
            Regex commentMatcher = new Regex(@"(?<Comment>\#.*$)", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

            // Ensure correct syntax
            var match = syntaxMatcher.Match(line);
            if (!match.Success)
            {
                _errors.Add(new AdsFileValidationError(_currentLineNumber, line, T(SyntaxNotUnderstoodError).ToString()));
                return;
            }
        }

        public List<AdsFileValidationError> Errors { get { return _errors; } }
        public bool IsValid { get { return _errors.Count == 0; } }
        public int CurrentLineNumber { get { return _currentLineNumber; } }

        #region IDisposable Members

        public void Dispose()
        {
            _reader.Dispose();
        }

        #endregion
    }

    public class AdsFileValidationError
    {
        public AdsFileValidationError(int lineNumber, string badContent, string error)
        {
            LineNumber = lineNumber;
            BadContent = badContent;
            Error = error;
        }
        public int LineNumber { get; private set; }
        public string BadContent { get; private set; }
        public string Error { get; private set; }
    }
}