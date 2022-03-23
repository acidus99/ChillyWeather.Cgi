using System;
using System.Linq;
using System.Globalization;
using System.IO;

using Chilly.Models;
using Chilly;


namespace ChillyCgi
{
    public class GeminiRenderer
    {
        TextWriter Fout;
        Formatter formatter;

        public GeminiRenderer(TextWriter fout)
        {
            Fout = fout;
            formatter = new Formatter();
        }

        public void Render(Forecast forecast)
        {
            formatter.IsMetric = forecast.IsMetric;


            Fout.WriteLine($"# Weather for {forecast.Location.Name} @ {formatter.FormatTime(forecast.Current.Time)}");

            Fout.WriteLine("=> /cgi-bin/chilly.cgi/search Wrong Location? Search");

            if (formatter.IsMetric)
            {
                Fout.WriteLine("=> ?f Use Fahrenheit");
            }
            else
            {
                Fout.WriteLine("=> ?c Use Celsius");
            }

            Fout.WriteLine($"Now: {formatter.EmojiForCurrentWeather(forecast.Current)} {formatter.FormatTemp(forecast.Current.Temp)} {formatter.FormatDescription(forecast.Current.Weather)}");
            Fout.WriteLine();
            Fout.WriteLine("## Next 24 hours");
            //TODO: add a summary here
            //skip every other hour
            int i = 0;
            foreach (HourlyCondition hour in forecast.Hourly.Skip(2).Take(24))
            {
                i++;
                if(i % 2 ==0)
                {
                    continue;
                }

                Fout.Write($"* {formatter.FormatHour(hour.Time)}: {formatter.EmojiForWeather(hour.Weather.Type)} {formatter.FormatTemp(hour.Temp)} ");
                if(hour.ChanceOfPrecipitation != 0)
                {
                    Fout.Write($"💧 {formatter.FormatChance(hour.ChanceOfPrecipitation)} - ");
                    
                }
                Fout.WriteLine(formatter.FormatDescription(hour.Weather));
            }

            Fout.WriteLine();

            Fout.WriteLine("## Next 7 Days");
            foreach(DailyCondition daily in forecast.Daily)
            {
                if (daily.Time < forecast.Current.Time)
                {
                    continue;
                }
                Fout.WriteLine($"{formatter.FormatDay(forecast.Current.Time, daily.Time)}: {formatter.EmojiForWeather(daily.Weather.Type)} {formatter.FormatTemp(daily.LowTemp)} to {formatter.FormatTemp(daily.HighTemp)}");
                if (daily.ChanceOfPrecipitation == 0)
                {
                    Fout.WriteLine($"\t{formatter.FormatDescription(daily.Weather)}");
                }
                else
                {
                    Fout.WriteLine($"\t💧 {formatter.FormatChance(daily.ChanceOfPrecipitation)} - {formatter.FormatDescription(daily.Weather)}");
                }
                Fout.WriteLine();
            }
        }
    }
}
