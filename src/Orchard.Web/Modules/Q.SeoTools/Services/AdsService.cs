using Orchard.Caching;
using Orchard.Data;
using Orchard.Localization;
using Q.SeoTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Q.SeoTools.Services
{
    public class AdsService : IAdsService
    {
        private const string DefaultRobotsText = @"google.com, pub-0000000000000000, DIRECT, f08c47fec0942fa0";

        private readonly IRepository<AdsFileRecord> _repository;
        private readonly ISignals _signals;

        public AdsService(IRepository<AdsFileRecord> repository, ISignals signals)
        {
            _repository = repository;
            _signals = signals;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public AdsFileRecord Get()
        {
            var adsFileRecord = _repository.Table.FirstOrDefault();
            if (adsFileRecord == null)
            {
                adsFileRecord = new AdsFileRecord()
                {
                    FileContent = DefaultRobotsText
                };
                _repository.Create(adsFileRecord);
            }
            return adsFileRecord;
        }

        public Tuple<bool, IEnumerable<string>> Save(string text)
        {
            var adsFileRecord = Get();
            adsFileRecord.FileContent = text;
            _signals.Trigger("AdsFile.SettingsChanged");
            var validationResult = Validate(text);
            return validationResult;
        }

        private Tuple<bool, IEnumerable<string>> Validate(string text)
        {
            using (var validatingReader = new RobotsFileValidatingReader(text))
            {
                validatingReader.ValidateToEnd();
                return new Tuple<bool, IEnumerable<string>>(validatingReader.IsValid, validatingReader.Errors.Select(error =>
                    T("Line {0}: {1}, {2}", error.LineNumber, error.BadContent, error.Error).ToString()
                ));
            }
        }
    }
}