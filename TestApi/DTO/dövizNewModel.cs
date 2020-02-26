namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class dövizNewModel
    {
        [JsonProperty("Güncelleme Tarihi")]
        public DateTimeOffset GüncellemeTarihi { get; set; }

        [JsonProperty("ABD DOLARI")]
        public PuneHedgehog AbdDolari { get; set; }

        [JsonProperty("AVUSTRALYA DOLARI")]
        public PuneHedgehog AvustralyaDolari { get; set; }

        [JsonProperty("DANİMARKA KRONU")]
        public PuneHedgehog Dani̇markaKronu { get; set; }

        [JsonProperty("EURO")]
        public PuneHedgehog Euro { get; set; }

        [JsonProperty("İNGİLİZ STERLİNİ")]
        public PuneHedgehog İngi̇li̇zSterli̇ni̇ { get; set; }

        [JsonProperty("İSVİÇRE FRANGI")]
        public PuneHedgehog İsvi̇çreFrangi { get; set; }

        [JsonProperty("İSVEÇ KRONU")]
        public PuneHedgehog İsveçKronu { get; set; }

        [JsonProperty("KANADA DOLARI")]
        public PuneHedgehog KanadaDolari { get; set; }

        [JsonProperty("KUVEYT DİNARI")]
        public PuneHedgehog KuveytDi̇nari { get; set; }

        [JsonProperty("NORVEÇ KRONU")]
        public PuneHedgehog NorveçKronu { get; set; }

        [JsonProperty("SUUDİ ARABİSTAN RİYALİ")]
        public PuneHedgehog Suudi̇Arabi̇stanRi̇yali̇ { get; set; }

        [JsonProperty("JAPON YENİ")]
        public PuneHedgehog JaponYeni̇ { get; set; }

        [JsonProperty("BULGAR LEVASI")]
        public PuneHedgehog BulgarLevasi { get; set; }

        [JsonProperty("RUMEN LEYİ")]
        public PuneHedgehog RumenLeyi̇ { get; set; }

        [JsonProperty("RUS RUBLESİ")]
        public PuneHedgehog RusRublesi̇ { get; set; }

        [JsonProperty("İRAN RİYALİ")]
        public PuneHedgehog İranRi̇yali̇ { get; set; }

        [JsonProperty("ÇİN YUANI")]
        public PuneHedgehog Çi̇nYuani { get; set; }

        [JsonProperty("PAKİSTAN RUPİSİ")]
        public PuneHedgehog Paki̇stanRupi̇si̇ { get; set; }

        [JsonProperty("KATAR RİYALİ")]
        public PuneHedgehog KatarRi̇yali̇ { get; set; }

        [JsonProperty("ÖZEL ÇEKME HAKKI (SDR)")]
        public ÖzelÇekmeHakkiSdr ÖzelÇekmeHakkiSdr { get; set; }

        [JsonProperty("Çeyrek Altın")]
        public PuneHedgehog ÇeyrekAltın { get; set; }

        [JsonProperty("Yarım Altın")]
        public PuneHedgehog YarımAltın { get; set; }

        [JsonProperty("Tam Altın")]
        public PuneHedgehog TamAltın { get; set; }

        [JsonProperty("Cumhuriyet Altını")]
        public PuneHedgehog CumhuriyetAltını { get; set; }

        [JsonProperty("Ons")]
        public PuneHedgehog Ons { get; set; }

        [JsonProperty("Gram Altın")]
        public PuneHedgehog GramAltın { get; set; }

        [JsonProperty("Ata Altın")]
        public PuneHedgehog AtaAltın { get; set; }

        [JsonProperty("14 Ayar Altın")]
        public PuneHedgehog The14AyarAltın { get; set; }

        [JsonProperty("18 Ayar Altın")]
        public PuneHedgehog The18AyarAltın { get; set; }

        [JsonProperty("22 Ayar Bilezik")]
        public PuneHedgehog The22AyarBilezik { get; set; }

        [JsonProperty("İkibuçuk Altın")]
        public PuneHedgehog İkibuçukAltın { get; set; }

        [JsonProperty("Beşli Altın")]
        public PuneHedgehog BeşliAltın { get; set; }

        [JsonProperty("Gremse Altın")]
        public PuneHedgehog GremseAltın { get; set; }

        [JsonProperty("Gümüş")]
        public PuneHedgehog Gümüş { get; set; }
    }

    public partial class PuneHedgehog
    {
        [JsonProperty("Alış")]
        public string Alış { get; set; }

        [JsonProperty("Satış")]
        public string Satış { get; set; }

        [JsonProperty("Tür")]
        public Tür Tür { get; set; }
    }

    public partial class ÖzelÇekmeHakkiSdr
    {
        [JsonProperty("Alış")]
        public string Alış { get; set; }

        [JsonProperty("Satış")]
        public object[] Satış { get; set; }

        [JsonProperty("Tür")]
        public Tür Tür { get; set; }
    }

    public enum Tür { Altın, Döviz };

    public partial class dövizNewModel
    {
        public static dövizNewModel FromJson(string json) => JsonConvert.DeserializeObject<dövizNewModel>(json, QuickType.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this dövizNewModel self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                TürConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class TürConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Tür) || t == typeof(Tür?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Altın":
                    return Tür.Altın;
                case "Döviz":
                    return Tür.Döviz;
            }
            throw new Exception("Cannot unmarshal type Tür");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Tür)untypedValue;
            switch (value)
            {
                case Tür.Altın:
                    serializer.Serialize(writer, "Altın");
                    return;
                case Tür.Döviz:
                    serializer.Serialize(writer, "Döviz");
                    return;
            }
            throw new Exception("Cannot marshal type Tür");
        }

        public static readonly TürConverter Singleton = new TürConverter();
    }
}
