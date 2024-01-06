namespace Bwr.Exchange.Models.Statments
{
    public class PdfResultOutput
    {
        public string Path { get; set; }

        public PdfResultOutput(string path)
        {
            Path = path;
        }
    }
}
