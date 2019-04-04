using System.IO;
using System.Linq;

namespace vega.Core.Models
{
    public class PhotosSettings
    {
        public int MaxBytes { get; set; }
        public string[] AcceptedFormats { get; set; }

        public bool IsValidFormat(string fileName)
        {
            return AcceptedFormats.Any(s=> s ==Path.GetExtension(fileName).ToLower());
        }
    }
}