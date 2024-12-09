namespace Coercer_dotnet.structures
{
    public class Hash
    {
        public string Nt { get; }
        public string? Lm { get; }

        public static Hash Parse(string hash)
        {
            return new Hash(hash);
        }

        public Hash(string hash)
        {
            string[] hashParts = hash.Split(":");
            if (hashParts.Length > 1)
            {
                Lm = hashParts[0];
                Nt = hashParts[1];
            }
            else
            {
                Nt = hash;
            }
        }

        public Hash(string nt, string lm)
        {
            Nt = nt;
            Lm = lm;
        }
    }
}