using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.Bots
{
    using System.Threading;

    public class ValidCreditCardMaker
    {
        // 'prefix' is the start of  CC number as a string, any number
		// private of digits 'length' is the length of the CC number to generate.
	    // Typically 13 or	16
		
        private static string CreateFakeCreditCardNumber(string prefix, int length)
        {
            string ccnumber = prefix;

            while (ccnumber.Length < (length - 1))
            {
                double rnd = (new Random().NextDouble() * 1.0f - 0f);

                ccnumber += Math.Floor(rnd * 10);

                //sleep so we get a different seed

                Thread.Sleep(20);
            }


            // reverse number and convert to int
            var reversedCCnumberstring = ccnumber.ToCharArray().Reverse();

            var reversedCCnumberList = reversedCCnumberstring.Select(c => Convert.ToInt32(c.ToString()));

            // calculate sum

            int sum = 0;
            int pos = 0;
            int[] reversedCCnumber = reversedCCnumberList.ToArray();

            while (pos < length - 1)
            {
                int odd = reversedCCnumber[pos] * 2;

                if (odd > 9)
                    odd -= 9;

                sum += odd;

                if (pos != (length - 2))
                    sum += reversedCCnumber[pos + 1];

                pos += 2;
            }

            // calculate check digit
            int checkdigit =
                Convert.ToInt32((Math.Floor((decimal)sum / 10) + 1) * 10 - sum) % 10;

            ccnumber += checkdigit;

            return ccnumber;
        }


        private static IEnumerable<string> GetCreditCardNumbers(string[] prefixList, int length,
                                                  int howMany)
        {
            var result = new Stack<string>();

            for (int i = 0; i < howMany; i++)
            {
                int randomPrefix = new Random().Next(0, prefixList.Length - 1);

                if (randomPrefix > 1)
                {
                    randomPrefix--;
                }

                string ccnumber = prefixList[randomPrefix];

                result.Push(CreateFakeCreditCardNumber(ccnumber, length));
            }

            return result;
        }


        public static IEnumerable<string> GenerateRandomCreditCardNumbers(int howMany)
        {
            string[] prefix = PickRandomCreditCardPrefix();

            return GetCreditCardNumbers(_mastercardPrefixList, 16, howMany);
        }

        public static string GenerateRandomCreditCardNumber()
        {
            string[] prefix = PickRandomCreditCardPrefix();

            string generatedCard;
            do
            {
                generatedCard = GetCreditCardNumbers(_mastercardPrefixList, 16, 1).First();
            } while (!IsValidCreditCardNumber(generatedCard));

            return generatedCard;
        }

        private static string[] PickRandomCreditCardPrefix()
        {
            var listOfCreditCardPrefixes = new List<string[]>();
            listOfCreditCardPrefixes.Add(_amexPrefixList);
            listOfCreditCardPrefixes.Add(_dinersPrefixList);
            listOfCreditCardPrefixes.Add(_discoverPrefixList);
            listOfCreditCardPrefixes.Add(_enroutePrefixList);
            listOfCreditCardPrefixes.Add(_jcb15PrefixList);
            listOfCreditCardPrefixes.Add(_jcb16PrefixList);
            listOfCreditCardPrefixes.Add(_mastercardPrefixList);
            listOfCreditCardPrefixes.Add(_visaPrefixList);
            listOfCreditCardPrefixes.Add(_voyagerPrefixList);

            var randomCreditCardPrefixIndex = new Random().Next(0, listOfCreditCardPrefixes.Count);

            return listOfCreditCardPrefixes[randomCreditCardPrefixIndex];
        }

        public static bool IsValidCreditCardNumber(string creditCardNumber)
        {
            try
            {
                var reversedNumber = creditCardNumber.ToCharArray().Reverse();

                int mod10Count = 0;
                for (int i = 0; i < reversedNumber.Count(); i++)
                {
                    int augend = Convert.ToInt32(reversedNumber.ElementAt(i).ToString());

                    if (((i + 1) % 2) == 0)
                    {
                        string productstring = (augend * 2).ToString();
                        augend = 0;
                        for (int j = 0; j < productstring.Length; j++)
                        {
                            augend += Convert.ToInt32(productstring.ElementAt(j).ToString());
                        }
                    }
                    mod10Count += augend;
                }

                if ((mod10Count % 10) == 0)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        private static string[] _amexPrefixList = new[] { "34", "37" };


        private static string[] _dinersPrefixList = new[]
                                                        {
                                                            "300",
                                                            "301", "302", "303", "36", "38"
                                                        };


        private static string[] _discoverPrefixList = new[] { "6011" };


        private static string[] _enroutePrefixList = new[]
                                                         {
                                                             "2014",
                                                             "2149"
                                                         };


        private static string[] _jcb15PrefixList = new[]
                                                        {
                                                            "2100",
                                                            "1800"
                                                        };


        private static string[] _jcb16PrefixList = new[]
                                                        {
                                                            "3088",
                                                            "3096", "3112", "3158", "3337", "3528"
                                                        };


        private static string[] _mastercardPrefixList = new[]
                                                            {
                                                                "51",
                                                                "52", "53", "54", "55"
                                                            };


        private static string[] _visaPrefixList = new[]
                                                      {
                                                          "4539",
                                                          "4556", "4916", "4532", "4929", "40240071", "4485", "4716", "4"
                                                      };


        private static string[] _voyagerPrefixList = new[] { "8699" };
    }
}
