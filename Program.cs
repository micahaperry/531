using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net.Mail;
using System.Net;

namespace _531
{
    class Program
    {
        static void Main(string[] args)
        {
            // Month 4
            //deadlift, bench, squat, overhead, inclineBench, straightDeadlift, frontSquat, closeGripBench
            Maxes month4MaxesMicah = new Maxes(273, 210, 265, 132, 150, 195, 165, 150, Lifter.Micah);
            Maxes month4MaxesMatt = new Maxes(273, 210, 285, 132, 150, 195, 165, 150, Lifter.Matt);
            List<Maxes> month4Maxes = new List<Maxes>() { month4MaxesMicah, month4MaxesMatt };

            // Month 3
            Maxes month3MaxesMicah = new Maxes(263, 210, 255, 127, 145, 185, 155, 145, Lifter.Micah);
            Maxes month3MaxesMatt = new Maxes(263, 210, 275, 127, 145, 185, 155, 145, Lifter.Matt);
            List<Maxes> month3Maxes = new List<Maxes>() { month3MaxesMicah, month3MaxesMatt };

            MonthlyWorkout(Week.WeekOne, month4Maxes, "MonthFourWeekOne", "531 -  Month Four - Week One");

            //MonthlyWorkout(Week.WeekThree, month3Maxes, "MonthThreeWeekThree", "531 -  Month Three - Week Three");

            //MonthlyWorkout(Week.WeekOne, maxes, "MonthThreeWeekOne", "531 -  Month Three - Week One");

            //MonthThreeWeekOne(month3MaxesMicah, month3MaxesMatt);
            //MonthThree(month3MaxesMicah, month3MaxesMatt); 
        }

        private static void MonthlyWorkout(Week week, List<Maxes> maxes, string filename, string description)
        {
            string deadliftWeekOneMicah = CalculateDeadlift(week, maxes[0]);

            string benchWeekOne = CalculateBench(week, maxes[0]);

            string overheadWeekOne = CalculateOverhead(week, maxes[0]);

            string squatWeekOneMatt = CalculateSquat(week, maxes[0]);
            string squatWeekOneMicah = CalculateSquat(week, maxes[1]);

            List<string> workouts = new List<string>() { deadliftWeekOneMicah, benchWeekOne, overheadWeekOne, squatWeekOneMatt, squatWeekOneMicah };
            PrintWorkouts(workouts, filename, description);
        }

        private static void MonthThreeWeekOne(Maxes micahMaxes, Maxes mattMaxes)
        {
            string deadliftWeekOneMicah = CalculateDeadlift(Week.WeekOne, micahMaxes);

            string benchWeekOne = CalculateBench(Week.WeekOne, micahMaxes);

            string overheadWeekOne = CalculateOverhead(Week.WeekOne, micahMaxes);

            string squatWeekOneMatt = CalculateSquat(Week.WeekOne, mattMaxes);
            string squatWeekOneMicah = CalculateSquat(Week.WeekOne, micahMaxes);

            List<string> workouts = new List<string>() { deadliftWeekOneMicah, benchWeekOne, overheadWeekOne, squatWeekOneMatt, squatWeekOneMicah };

            string filename = "MonthThreeWeekOne.txt";
            string description = "531 -  Month Three - Week One ";
            PrintWorkouts(workouts, filename, description );

            //PrintWorkout(deadliftWeekOneMicah + Environment.NewLine + benchWeekOne + Environment.NewLine + overheadWeekOne, "MonthThreeWeekOne", "Month Three");
        }

        private static void PrintWorkouts(List<string> workouts, string filename, string description)
        {
            using (StreamWriter streamWriter = new StreamWriter(filename))
            {
                streamWriter.WriteLine(description + Environment.NewLine);

                foreach(string workout in workouts)
                {
                    streamWriter.WriteLine(workout + Environment.NewLine);
                }
            }

            //string firstText = "Hello";
            //string secondText = "World";

            //PointF firstLocation = new PointF(10f, 10f);
            //PointF secondLocation = new PointF(10f, 50f);

            //string imageFilePath = @"path\picture.bmp"
            string imageFilePath = @"workouts\" + filename;

            List<string> imageFilepaths = new List<string>(); 

            int workoutNumber = 1;
            foreach (string workout in workouts)
            {
                Bitmap bitmap = (Bitmap)Image.FromFile("blank.bmp");//load the image file

                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Font arialFont = new Font("Arial", 10))
                    {
                        //streamWriter.WriteLine(description + Environment.NewLine);

                        //foreach (string workout in workouts)
                        //{
                        //    streamWriter.WriteLine(workout + Environment.NewLine);
                        //}

                        PointF firstLocation = new PointF(10f, 10f);

                        float xlocation = 10f;
                        float ylocation = 50f;

                        PointF workoutLocation = new PointF(xlocation, ylocation);

                        graphics.DrawString(description + Environment.NewLine, arialFont, Brushes.Black, firstLocation);

                        graphics.DrawString(workout + Environment.NewLine, arialFont, Brushes.Black, workoutLocation);

                        string imageFilepath = imageFilePath + workoutNumber + ".bmp";
                        imageFilepaths.Add(imageFilePath);

                        bitmap.Save(imageFilepath);

                        workoutNumber++;
                        ylocation += 50f;
                    }
                }
            }

            List<string> addresses = new List<string>();
            addresses.Add("micahaperry@gmail.com");

            //SendEmail(addresses, filename, imageFilepaths);

            // Open folder
            Process.Start("explorer.exe", "/select," + filename);
        }

        private static void SendEmail(List<string> addresses, string filename, List<string> imageFilepaths)
        {
            // TODO: Send email
            //   gmail: http://stackoverflow.com/questions/32260/sending-email-in-net-through-gmail
            //   attachments: http://stackoverflow.com/questions/2825950/sending-email-with-attachments-from-c-attachments-arrive-as-part-1-2-in-thunde  


            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("micahaperryip@gmail.com");
            msg.To.Add(addresses[0]);
            msg.Subject = "Hello world! " + DateTime.Now.ToString();
            msg.Body = "hi to you ... :)";
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Host = "smtp.gmail.com";
            client.Port = 25;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("micahaperryip@gmail.com", "ball4316");
            client.Timeout = 20000;
            try
            {
                client.Send(msg);
                //return "Mail has been successfully sent!";
            }
            catch (Exception ex)
            {
                //return "Fail Has error" + ex.Message;
            }
            finally
            {
                msg.Dispose();
            }
        }

        private static void MonthThree(Maxes micahMaxes, Maxes mattMaxes)
        {
            string deadliftWeekOne = CalculateDeadlift(Week.WeekOne, micahMaxes);
            string benchWeekOne = CalculateBench(Week.WeekOne, micahMaxes);

            string deadliftWeekTwo = CalculateDeadlift(Week.WeekTwo, micahMaxes);
            string benchWeekTwo = CalculateBench(Week.WeekTwo, micahMaxes);

            string deadliftWeekThree = CalculateDeadlift(Week.WeekThree, micahMaxes);
            string benchWeekThree = CalculateBench(Week.WeekThree, micahMaxes);

            string deadliftWeekFour = CalculateDeadlift(Week.WeekFour, micahMaxes);
            string benchWeekFour = CalculateBench(Week.WeekFour, micahMaxes);

            PrintWorkout(benchWeekOne + Environment.NewLine + deadliftWeekOne, "MonthThreeWeekOne", "Month Three");
            PrintWorkout(benchWeekTwo + Environment.NewLine + deadliftWeekTwo, "MonthThreeWeekTwo", "Month Three");
            PrintWorkout(benchWeekThree + Environment.NewLine + deadliftWeekThree, "MonthThreeWeekThree", "Month Three");
            PrintWorkout(benchWeekFour + Environment.NewLine + deadliftWeekFour, "MonthThreeWeekFour", "Month Three");
        }

        private static string CalculateSquat(Week week, Maxes maxes, bool printName = false)
        {
            // TODO: Change every workout to programmatically print names
            printName = true;
            string squatCalculations = string.Empty;
            if(printName)
            {
                squatCalculations = week.ToString() + " Squat " + maxes.Squat.ToString() + " 1RM " + "(" + maxes.Person.ToString() + ")" + Environment.NewLine;
            }
            else
            {
                squatCalculations = week.ToString() + " Squat " + maxes.Squat.ToString() + " 1RM " + Environment.NewLine;
            }

            double mainLiftSetOne = 0;
            double mainLiftSetTwo = 0;
            double mainLiftSetThree = 0;

            double assistanceLiftSetOne = 0;
            double assistanceLiftSetTwo = 0;
            double assistanceLiftSetThree = 0;

            if (week == Week.WeekOne)
            {
                mainLiftSetOne = Mainlift.Percentages.WeekOne.setOne;
                mainLiftSetTwo = Mainlift.Percentages.WeekOne.setTwo;
                mainLiftSetThree = Mainlift.Percentages.WeekOne.setThree;

                assistanceLiftSetOne = AssistanceLift.Percentages.WeekOne.setOne;
                assistanceLiftSetTwo = AssistanceLift.Percentages.WeekOne.setTwo;
                assistanceLiftSetThree = AssistanceLift.Percentages.WeekOne.setThree;
            }
            else if (week == Week.WeekTwo)
            {
                mainLiftSetOne = Mainlift.Percentages.WeekTwo.setOne;
                mainLiftSetTwo = Mainlift.Percentages.WeekTwo.setTwo;
                mainLiftSetThree = Mainlift.Percentages.WeekTwo.setThree;

                assistanceLiftSetOne = AssistanceLift.Percentages.WeekTwo.setOne;
                assistanceLiftSetTwo = AssistanceLift.Percentages.WeekTwo.setTwo;
                assistanceLiftSetThree = AssistanceLift.Percentages.WeekTwo.setThree;
            }
            else if (week == Week.WeekThree)
            {
                mainLiftSetOne = Mainlift.Percentages.WeekThree.setOne;
                mainLiftSetTwo = Mainlift.Percentages.WeekThree.setTwo;
                mainLiftSetThree = Mainlift.Percentages.WeekThree.setThree;

                assistanceLiftSetOne = AssistanceLift.Percentages.WeekThree.setOne;
                assistanceLiftSetTwo = AssistanceLift.Percentages.WeekThree.setTwo;
                assistanceLiftSetThree = AssistanceLift.Percentages.WeekThree.setThree;
            }
            else if (week == Week.WeekFour)
            {
                mainLiftSetOne = Mainlift.Percentages.WeekFour.setOne;
                mainLiftSetTwo = Mainlift.Percentages.WeekFour.setTwo;
                mainLiftSetThree = Mainlift.Percentages.WeekFour.setThree;

                assistanceLiftSetOne = AssistanceLift.Percentages.WeekFour.setOne;
                assistanceLiftSetTwo = AssistanceLift.Percentages.WeekFour.setTwo;
                assistanceLiftSetThree = AssistanceLift.Percentages.WeekFour.setThree;
            }

            // Warmup
            double squatWarmupSet1 = maxes.Squat * Mainlift.Percentages.Warmup.setOne;
            double squatWarmupSet2 = maxes.Squat * Mainlift.Percentages.Warmup.setTwo;
            double squatWarmupSet3 = maxes.Squat * Mainlift.Percentages.Warmup.setThree;

            List<double> platesSquatWarmupSet1 = Plates.SelectPlates(squatWarmupSet1, Exercise.Squat);
            List<double> platesSquatWarmupSet2 = Plates.SelectPlates(squatWarmupSet2, Exercise.Squat);
            List<double> platesSquatWarmupSet3 = Plates.SelectPlates(squatWarmupSet3, Exercise.Squat);

            Dictionary<double, List<double>> warmupDictionary = new Dictionary<double, List<double>>()
            {
                { squatWarmupSet1, platesSquatWarmupSet1},
                { squatWarmupSet2, platesSquatWarmupSet2},
                { squatWarmupSet3, platesSquatWarmupSet3},
            };

            string warmupString = FormatWarmup(warmupDictionary);

            // Sets
            string sets = string.Empty;

            double squatLiftSet1 = maxes.Squat * mainLiftSetOne;
            double squatLiftSet2 = maxes.Squat * mainLiftSetTwo;
            double squatLiftSet3 = maxes.Squat * mainLiftSetThree;

            List<double> platesSquatSet1 = Plates.SelectPlates(squatLiftSet1, Exercise.Squat);
            List<double> platesSquatSet2 = Plates.SelectPlates(squatLiftSet2, Exercise.Squat);
            List<double> platesSquatSet3 = Plates.SelectPlates(squatLiftSet3, Exercise.Squat);

            Dictionary<double, List<double>> setDictionary = new Dictionary<double, List<double>>()
            {
                { squatLiftSet1, platesSquatSet1},
                { squatLiftSet2, platesSquatSet2},
                { squatLiftSet3, platesSquatSet3},
            };

            string setsString = FormatSets(setDictionary, week);

            // Assistance
            double straightDeadliftSet1 = maxes.StraightDeadlift * assistanceLiftSetOne;
            double straightDeadliftSet2 = maxes.StraightDeadlift * assistanceLiftSetTwo;
            double straightDeadliftSet3 = maxes.StraightDeadlift * assistanceLiftSetThree;

            List<double> platesStraightDeadliftSet1 = Plates.SelectPlates(straightDeadliftSet1, Exercise.StraightDeadlift);
            List<double> platesStraightDeadliftSet2 = Plates.SelectPlates(straightDeadliftSet2, Exercise.StraightDeadlift);
            List<double> platesStraightDeadliftSet3 = Plates.SelectPlates(straightDeadliftSet3, Exercise.StraightDeadlift);

            Dictionary<double, List<double>> assistanceDictionary = new Dictionary<double, List<double>>()
            {
                { straightDeadliftSet1, platesStraightDeadliftSet1},
                { straightDeadliftSet2, platesStraightDeadliftSet2},
                { straightDeadliftSet3, platesStraightDeadliftSet3},
            };

            string assistanceString = FormatAssistance(assistanceDictionary, week, "Straight-Leg Deadlift " + maxes.StraightDeadlift + " 1RM");

            squatCalculations += warmupString + setsString + assistanceString;

            return squatCalculations;
        }

        private static string CalculateOverhead(Week week, Maxes maxes, bool printName = false)
        {
            //string overheadCalculations = week.ToString() + " Overhead " + maxes.Overhead.ToString() + " 1RM " + "(" + maxes.Person.ToString() + ")" + Environment.NewLine;

            // TODO: Change every workout to programmatically print names
            printName = false;
            string overheadCalculations = string.Empty;
            if (printName)
            {
                overheadCalculations = week.ToString() + " Overhead " + maxes.Overhead.ToString() + " 1RM " + "(" + maxes.Person.ToString() + ")" + Environment.NewLine;
            }
            else
            {
                overheadCalculations = week.ToString() + " Overhead " + maxes.Overhead.ToString() + " 1RM " + Environment.NewLine;
            }

            double mainLiftSetOne = 0;
            double mainLiftSetTwo = 0;
            double mainLiftSetThree = 0;

            double assistanceLiftSetOne = 0;
            double assistanceLiftSetTwo = 0;
            double assistanceLiftSetThree = 0;

            if (week == Week.WeekOne)
            {
                mainLiftSetOne = Mainlift.Percentages.WeekOne.setOne;
                mainLiftSetTwo = Mainlift.Percentages.WeekOne.setTwo;
                mainLiftSetThree = Mainlift.Percentages.WeekOne.setThree;

                assistanceLiftSetOne = AssistanceLift.Percentages.WeekOne.setOne;
                assistanceLiftSetTwo = AssistanceLift.Percentages.WeekOne.setTwo;
                assistanceLiftSetThree = AssistanceLift.Percentages.WeekOne.setThree;
            }
            else if (week == Week.WeekTwo)
            {
                mainLiftSetOne = Mainlift.Percentages.WeekTwo.setOne;
                mainLiftSetTwo = Mainlift.Percentages.WeekTwo.setTwo;
                mainLiftSetThree = Mainlift.Percentages.WeekTwo.setThree;

                assistanceLiftSetOne = AssistanceLift.Percentages.WeekTwo.setOne;
                assistanceLiftSetTwo = AssistanceLift.Percentages.WeekTwo.setTwo;
                assistanceLiftSetThree = AssistanceLift.Percentages.WeekTwo.setThree;
            }
            else if (week == Week.WeekThree)
            {
                mainLiftSetOne = Mainlift.Percentages.WeekThree.setOne;
                mainLiftSetTwo = Mainlift.Percentages.WeekThree.setTwo;
                mainLiftSetThree = Mainlift.Percentages.WeekThree.setThree;

                assistanceLiftSetOne = AssistanceLift.Percentages.WeekThree.setOne;
                assistanceLiftSetTwo = AssistanceLift.Percentages.WeekThree.setTwo;
                assistanceLiftSetThree = AssistanceLift.Percentages.WeekThree.setThree;
            }
            else if (week == Week.WeekFour)
            {
                mainLiftSetOne = Mainlift.Percentages.WeekFour.setOne;
                mainLiftSetTwo = Mainlift.Percentages.WeekFour.setTwo;
                mainLiftSetThree = Mainlift.Percentages.WeekFour.setThree;

                assistanceLiftSetOne = AssistanceLift.Percentages.WeekFour.setOne;
                assistanceLiftSetTwo = AssistanceLift.Percentages.WeekFour.setTwo;
                assistanceLiftSetThree = AssistanceLift.Percentages.WeekFour.setThree;
            }

            // Warmup
            double overheadWarmupSet1 = maxes.Overhead * Mainlift.Percentages.Warmup.setOne;
            double overheadWarmupSet2 = maxes.Overhead * Mainlift.Percentages.Warmup.setTwo;
            double overheadWarmupSet3 = maxes.Overhead * Mainlift.Percentages.Warmup.setThree;

            List<double> platesOverheadWarmupSet1 = Plates.SelectPlates(overheadWarmupSet1, Exercise.Overhead);
            List<double> platesOverheadWarmupSet2 = Plates.SelectPlates(overheadWarmupSet2, Exercise.Overhead);
            List<double> platesOverheadWarmupSet3 = Plates.SelectPlates(overheadWarmupSet3, Exercise.Overhead);

            Dictionary<double, List<double>> warmupDictionary = new Dictionary<double, List<double>>()
            {
                { overheadWarmupSet1, platesOverheadWarmupSet1},
                { overheadWarmupSet2, platesOverheadWarmupSet2},
                { overheadWarmupSet3, platesOverheadWarmupSet3},
            };

            string warmupString = FormatWarmup(warmupDictionary);

            // Sets
            string sets = string.Empty;

            double overheadLiftSet1 = maxes.Overhead * mainLiftSetOne;
            double overheadLiftSet2 = maxes.Overhead * mainLiftSetTwo;
            double overheadLiftSet3 = maxes.Overhead * mainLiftSetThree;

            List<double> platesOverheadSet1 = Plates.SelectPlates(overheadLiftSet1, Exercise.Overhead);
            List<double> platesOverheadSet2 = Plates.SelectPlates(overheadLiftSet2, Exercise.Overhead);
            List<double> platesOverheadSet3 = Plates.SelectPlates(overheadLiftSet3, Exercise.Overhead);

            Dictionary<double, List<double>> setDictionary = new Dictionary<double, List<double>>()
            {
                { overheadLiftSet1, platesOverheadSet1},
                { overheadLiftSet2, platesOverheadSet2},
                { overheadLiftSet3, platesOverheadSet3},
            };

            string setsString = FormatSets(setDictionary, week);

            // Assistance
            double closeGripSet1 = maxes.CloseGripBench * assistanceLiftSetOne;
            double closeGripSet2 = maxes.CloseGripBench * assistanceLiftSetTwo;
            double closeGripSet3 = maxes.CloseGripBench * assistanceLiftSetThree;

            List<double> platesCloseGripSet1 = Plates.SelectPlates(closeGripSet1, Exercise.CloseGripBench);
            List<double> platesCloseGripSet2 = Plates.SelectPlates(closeGripSet2, Exercise.CloseGripBench);
            List<double> platesCloseGripSet3 = Plates.SelectPlates(closeGripSet3, Exercise.CloseGripBench);

            Dictionary<double, List<double>> assistanceDictionary = new Dictionary<double, List<double>>()
            {
                { closeGripSet1, platesCloseGripSet1},
                { closeGripSet2, platesCloseGripSet2},
                { closeGripSet3, platesCloseGripSet3},
            };

            string assistanceString = FormatAssistance(assistanceDictionary, week, "Close Grip Bench Press " + maxes.CloseGripBench + " 1RM");

            overheadCalculations += warmupString + setsString + assistanceString;

            return overheadCalculations;
        }

        private static string CalculateBench(Week week, Maxes maxes, bool printName = false)
        {
            //string benchCalculations = week.ToString() + " Bench " + maxes.Bench.ToString() + " 1RM " + "(" + maxes.Person.ToString() + ")" + Environment.NewLine;

            // TODO: Change every workout to programmatically print names
            printName = false;
            string benchCalculations = string.Empty;
            if (printName)
            {
                benchCalculations = week.ToString() + " Bench " + maxes.Bench.ToString() + " 1RM " + "(" + maxes.Person.ToString() + ")" + Environment.NewLine;
            }
            else
            {
                benchCalculations = week.ToString() + " Bench " + maxes.Bench.ToString() + " 1RM " + Environment.NewLine;
            }

            double mainLiftSetOne = 0;
            double mainLiftSetTwo = 0;
            double mainLiftSetThree = 0;

            double assistanceLiftSetOne = 0;
            double assistanceLiftSetTwo = 0;
            double assistanceLiftSetThree = 0;

            if (week == Week.WeekOne)
            {
                mainLiftSetOne = Mainlift.Percentages.WeekOne.setOne;
                mainLiftSetTwo = Mainlift.Percentages.WeekOne.setTwo;
                mainLiftSetThree = Mainlift.Percentages.WeekOne.setThree;

                assistanceLiftSetOne = AssistanceLift.Percentages.WeekOne.setOne;
                assistanceLiftSetTwo = AssistanceLift.Percentages.WeekOne.setTwo;
                assistanceLiftSetThree = AssistanceLift.Percentages.WeekOne.setThree;
            }
            else if (week == Week.WeekTwo)
            {
                mainLiftSetOne = Mainlift.Percentages.WeekTwo.setOne;
                mainLiftSetTwo = Mainlift.Percentages.WeekTwo.setTwo;
                mainLiftSetThree = Mainlift.Percentages.WeekTwo.setThree;

                assistanceLiftSetOne = AssistanceLift.Percentages.WeekTwo.setOne;
                assistanceLiftSetTwo = AssistanceLift.Percentages.WeekTwo.setTwo;
                assistanceLiftSetThree = AssistanceLift.Percentages.WeekTwo.setThree;
            }
            else if (week == Week.WeekThree)
            {
                mainLiftSetOne = Mainlift.Percentages.WeekThree.setOne;
                mainLiftSetTwo = Mainlift.Percentages.WeekThree.setTwo;
                mainLiftSetThree = Mainlift.Percentages.WeekThree.setThree;

                assistanceLiftSetOne = AssistanceLift.Percentages.WeekThree.setOne;
                assistanceLiftSetTwo = AssistanceLift.Percentages.WeekThree.setTwo;
                assistanceLiftSetThree = AssistanceLift.Percentages.WeekThree.setThree;
            }
            else if (week == Week.WeekFour)
            {
                mainLiftSetOne = Mainlift.Percentages.WeekFour.setOne;
                mainLiftSetTwo = Mainlift.Percentages.WeekFour.setTwo;
                mainLiftSetThree = Mainlift.Percentages.WeekFour.setThree;

                assistanceLiftSetOne = AssistanceLift.Percentages.WeekFour.setOne;
                assistanceLiftSetTwo = AssistanceLift.Percentages.WeekFour.setTwo;
                assistanceLiftSetThree = AssistanceLift.Percentages.WeekFour.setThree;
            }

            // Warmup
            double benchWarmupSet1 = maxes.Bench * Mainlift.Percentages.Warmup.setOne;
            double benchWarmupSet2 = maxes.Bench * Mainlift.Percentages.Warmup.setTwo;
            double benchWarmupSet3 = maxes.Bench * Mainlift.Percentages.Warmup.setThree;

            List<double> platesBenchWarmupSet1 = Plates.SelectPlates(benchWarmupSet1, Exercise.Bench);
            List<double> platesBenchWarmupSet2 = Plates.SelectPlates(benchWarmupSet2, Exercise.Bench);
            List<double> platesBenchWarmupSet3 = Plates.SelectPlates(benchWarmupSet3, Exercise.Bench);

            Dictionary<double, List<double>> warmupDictionary = new Dictionary<double, List<double>>()
            {
                { benchWarmupSet1, platesBenchWarmupSet1},
                { benchWarmupSet2, platesBenchWarmupSet2},
                { benchWarmupSet3, platesBenchWarmupSet3},
            };

            string warmupString = FormatWarmup(warmupDictionary);

            // Sets
            string sets = string.Empty;

            double benchliftSet1 = maxes.Bench * mainLiftSetOne;
            double benchliftSet2 = maxes.Bench * mainLiftSetTwo;
            double benchliftSet3 = maxes.Bench * mainLiftSetThree;

            List<double> platesBenchSet1 = Plates.SelectPlates(benchliftSet1, Exercise.Bench);
            List<double> platesBenchSet2 = Plates.SelectPlates(benchliftSet2, Exercise.Bench);
            List<double> platesBenchSet3 = Plates.SelectPlates(benchliftSet3, Exercise.Bench);

            Dictionary<double, List<double>> setDictionary = new Dictionary<double, List<double>>()
            {
                { benchliftSet1, platesBenchSet1},
                { benchliftSet2, platesBenchSet2},
                { benchliftSet3, platesBenchSet3},
            };

            string setsString = FormatSets(setDictionary, week);

            // Assistance
            double inclineSet1 = maxes.InclineBench * assistanceLiftSetOne;
            double inclineSet2 = maxes.InclineBench * assistanceLiftSetTwo;
            double inclineSet3 = maxes.InclineBench * assistanceLiftSetThree;

            List<double> platesInclineSet1 = Plates.SelectPlates(inclineSet1, Exercise.InclineBench);
            List<double> platesInclineSet2 = Plates.SelectPlates(inclineSet2, Exercise.InclineBench);
            List<double> platesInclineSet3 = Plates.SelectPlates(inclineSet3, Exercise.InclineBench);

            Dictionary<double, List<double>> assistanceDictionary = new Dictionary<double, List<double>>()
            {
                { inclineSet1, platesInclineSet1},
                { inclineSet2, platesInclineSet2},
                { inclineSet3, platesInclineSet3},
            };

            string assistanceString = FormatAssistance(assistanceDictionary, week, "Incline Bench " + maxes.InclineBench + " 1RM");

            benchCalculations += warmupString + setsString + assistanceString;

            return benchCalculations;
        }

        private static void PrintWorkout(string deadliftWeekOne, string filename, string description = "Workout Program")
        {
            using (StreamWriter streamWriter = new StreamWriter(filename))
            {
                streamWriter.WriteLine(deadliftWeekOne);
            }
        }

        public static string CalculateDeadlift(Week week, Maxes maxes, bool printName = false)
        {
            //string deadliftCalculations = week.ToString() + " Deadlift " + maxes.Deadlift.ToString() + " 1RM " + "(" + maxes.Person.ToString() + ")" + Environment.NewLine;

            // TODO: Change every workout to programmatically print names
            printName = false;
            string deadliftCalculations = string.Empty;
            if (printName)
            {
                deadliftCalculations = week.ToString() + " Deadlift " + maxes.Deadlift.ToString() + " 1RM " + "(" + maxes.Person.ToString() + ")" + Environment.NewLine;
            }
            else
            {
                deadliftCalculations = week.ToString() + " Deadlift " + maxes.Deadlift.ToString() + " 1RM " + Environment.NewLine;
            }

            double mainLiftSetOne = 0;
            double mainLiftSetTwo = 0;
            double mainLiftSetThree = 0;

            double assistanceLiftSetOne = 0;
            double assistanceLiftSetTwo = 0;
            double assistanceLiftSetThree = 0;

            if (week == Week.WeekOne)
            {
                mainLiftSetOne = Mainlift.Percentages.WeekOne.setOne;
                mainLiftSetTwo = Mainlift.Percentages.WeekOne.setTwo;
                mainLiftSetThree = Mainlift.Percentages.WeekOne.setThree;

                assistanceLiftSetOne = AssistanceLift.Percentages.WeekOne.setOne;
                assistanceLiftSetTwo = AssistanceLift.Percentages.WeekOne.setTwo;
                assistanceLiftSetThree = AssistanceLift.Percentages.WeekOne.setThree;
            }
            else if (week == Week.WeekTwo)
            {
                mainLiftSetOne = Mainlift.Percentages.WeekTwo.setOne;
                mainLiftSetTwo = Mainlift.Percentages.WeekTwo.setTwo;
                mainLiftSetThree = Mainlift.Percentages.WeekTwo.setThree;

                assistanceLiftSetOne = AssistanceLift.Percentages.WeekTwo.setOne;
                assistanceLiftSetTwo = AssistanceLift.Percentages.WeekTwo.setTwo;
                assistanceLiftSetThree = AssistanceLift.Percentages.WeekTwo.setThree;
            }
            else if (week == Week.WeekThree)
            {
                mainLiftSetOne = Mainlift.Percentages.WeekThree.setOne;
                mainLiftSetTwo = Mainlift.Percentages.WeekThree.setTwo;
                mainLiftSetThree = Mainlift.Percentages.WeekThree.setThree;

                assistanceLiftSetOne = AssistanceLift.Percentages.WeekThree.setOne;
                assistanceLiftSetTwo = AssistanceLift.Percentages.WeekThree.setTwo;
                assistanceLiftSetThree = AssistanceLift.Percentages.WeekThree.setThree;
            }
            else if (week == Week.WeekFour)
            {
                mainLiftSetOne = Mainlift.Percentages.WeekFour.setOne;
                mainLiftSetTwo = Mainlift.Percentages.WeekFour.setTwo;
                mainLiftSetThree = Mainlift.Percentages.WeekFour.setThree;

                assistanceLiftSetOne = AssistanceLift.Percentages.WeekFour.setOne;
                assistanceLiftSetTwo = AssistanceLift.Percentages.WeekFour.setTwo;
                assistanceLiftSetThree = AssistanceLift.Percentages.WeekFour.setThree;
            }

            // Warmup
            double deadliftWarmupSet1 = maxes.Deadlift * Mainlift.Percentages.Warmup.setOne;
            double deadliftWarmupSet2 = maxes.Deadlift * Mainlift.Percentages.Warmup.setTwo;
            double deadliftWarmupSet3 = maxes.Deadlift * Mainlift.Percentages.Warmup.setThree;

            List<double> platesDeadliftWarmupSet1 = Plates.SelectPlates(deadliftWarmupSet1, Exercise.Deadlift);
            List<double> platesDeadliftWarmupSet2 = Plates.SelectPlates(deadliftWarmupSet2, Exercise.Deadlift);
            List<double> platesDeadliftWarmupSet3 = Plates.SelectPlates(deadliftWarmupSet3, Exercise.Deadlift);

            Dictionary<double, List<double>> warmupDictionary = new Dictionary<double, List<double>>()
            {
                { deadliftWarmupSet1, platesDeadliftWarmupSet1},
                { deadliftWarmupSet2, platesDeadliftWarmupSet2},
                { deadliftWarmupSet3, platesDeadliftWarmupSet3},
            };

            string warmupString = FormatWarmup(warmupDictionary);

            // Sets
            string sets = string.Empty;

            double deadliftSet1 = maxes.Deadlift * mainLiftSetOne;
            double deadliftSet2 = maxes.Deadlift * mainLiftSetTwo;
            double deadliftSet3 = maxes.Deadlift * mainLiftSetThree;

            List<double> platesDeadliftSet1 = Plates.SelectPlates(deadliftSet1, Exercise.Deadlift);
            List<double> platesDeadliftSet2 = Plates.SelectPlates(deadliftSet2, Exercise.Deadlift);
            List<double> platesDeadliftSet3 = Plates.SelectPlates(deadliftSet3, Exercise.Deadlift);

            Dictionary<double, List<double>> setDictionary = new Dictionary<double, List<double>>()
            {
                { deadliftSet1, platesDeadliftSet1},
                { deadliftSet2, platesDeadliftSet2},
                { deadliftSet3, platesDeadliftSet3},
            };

            string setsString = FormatSets(setDictionary, week);

            // Assistance
            string frontSquat = string.Empty;

            double frontSquatSet1 = maxes.FrontSquat * assistanceLiftSetOne;
            double frontSquatSet2 = maxes.FrontSquat * assistanceLiftSetTwo;
            double frontSquatSet3 = maxes.FrontSquat * assistanceLiftSetThree;

            List<double> platesFrontSquatSet1 = Plates.SelectPlates(frontSquatSet1, Exercise.FrontSquat);
            List<double> platesFrontSquatSet2 = Plates.SelectPlates(frontSquatSet2, Exercise.FrontSquat);
            List<double> platesFrontSquatSet3 = Plates.SelectPlates(frontSquatSet3, Exercise.FrontSquat);

            Dictionary<double, List<double>> assistanceDictionary = new Dictionary<double, List<double>>()
            {
                { frontSquatSet1, platesFrontSquatSet1},
                { frontSquatSet2, platesFrontSquatSet2},
                { frontSquatSet3, platesFrontSquatSet3},
            };

            string assistanceString = FormatAssistance(assistanceDictionary, week, "Front Squat " + maxes.FrontSquat + " 1RM");

            deadliftCalculations += warmupString + setsString + assistanceString;

            return deadliftCalculations; 
        }

        private static string FormatAssistance(Dictionary<double, List<double>> assistanceDictionary, Week week, string description)
        {
            string assistanceString = "Assistance: " + description + Environment.NewLine;

            int set = 1;
            foreach (KeyValuePair<double, List<double>> entry in assistanceDictionary)
            {
                string rep = string.Empty;
                switch (set)
                {
                    case 1:
                        if (week == Week.WeekOne)
                        {
                            rep = AssistanceLift.Reps.WeekOne.setOne.ToString();
                        }
                        else if (week == Week.WeekTwo)
                        {
                            rep = AssistanceLift.Reps.WeekTwo.setOne.ToString();
                        }
                        else if (week == Week.WeekThree)
                        {
                            rep = AssistanceLift.Reps.WeekThree.setOne.ToString();
                        }
                        else if (week == Week.WeekFour)
                        {
                            rep = AssistanceLift.Reps.WeekFour.setOne.ToString();
                        }

                        break;
                    case 2:
                        if (week == Week.WeekOne)
                        {
                            rep = AssistanceLift.Reps.WeekOne.setTwo.ToString();
                        }
                        else if (week == Week.WeekTwo)
                        {
                            rep = AssistanceLift.Reps.WeekTwo.setTwo.ToString();
                        }
                        else if (week == Week.WeekThree)
                        {
                            rep = AssistanceLift.Reps.WeekThree.setTwo.ToString();
                        }
                        else if (week == Week.WeekFour)
                        {
                            rep = AssistanceLift.Reps.WeekFour.setTwo.ToString();
                        }
                        break;
                    case 3:
                        if (week == Week.WeekOne)
                        {
                            rep = AssistanceLift.Reps.WeekOne.setThree.ToString();
                        }
                        else if (week == Week.WeekTwo)
                        {
                            rep = AssistanceLift.Reps.WeekTwo.setThree.ToString();
                        }
                        else if (week == Week.WeekThree)
                        {
                            rep = AssistanceLift.Reps.WeekThree.setThree.ToString();
                        }
                        else if (week == Week.WeekFour)
                        {
                            rep = AssistanceLift.Reps.WeekFour.setThree.ToString();
                        }
                        break;
                }

                double roundedWeight = Math.Round(entry.Key / 5.0) * 5;

                assistanceString += "  " + roundedWeight + " x " + rep + ": " + FormatPlates(entry.Value);
                assistanceString += Environment.NewLine;

                set++;
            }

            return assistanceString;
        }

        private static string FormatSets(Dictionary<double, List<double>> setDictionary, Week week)
        {
            string setString = "Sets" + Environment.NewLine;

            int set = 1;
            foreach (KeyValuePair<double, List<double>> entry in setDictionary)
            {
                string rep = string.Empty;
                switch (set)
                {
                    case 1:
                        if(week == Week.WeekOne)
                        {
                            rep = Mainlift.Reps.WeekOne.setOne.ToString();
                        }
                        else if (week == Week.WeekTwo)
                        {
                            rep = Mainlift.Reps.WeekTwo.setOne.ToString();
                        }
                        else if (week == Week.WeekThree)
                        {
                            rep = Mainlift.Reps.WeekThree.setOne.ToString();
                        }
                        else if (week == Week.WeekFour)
                        {
                            rep = Mainlift.Reps.WeekFour.setOne.ToString();
                        }

                        break;
                    case 2:
                        if (week == Week.WeekOne)
                        {
                            rep = Mainlift.Reps.WeekOne.setTwo.ToString();
                        }
                        else if (week == Week.WeekTwo)
                        {
                            rep = Mainlift.Reps.WeekTwo.setTwo.ToString();
                        }
                        else if (week == Week.WeekThree)
                        {
                            rep = Mainlift.Reps.WeekThree.setTwo.ToString();
                        }
                        else if (week == Week.WeekFour)
                        {
                            rep = Mainlift.Reps.WeekFour.setTwo.ToString();
                        }
                        break;
                    case 3:
                        if (week == Week.WeekOne)
                        {
                            rep = Mainlift.Reps.WeekOne.setThree.ToString();
                        }
                        else if (week == Week.WeekTwo)
                        {
                            rep = Mainlift.Reps.WeekTwo.setThree.ToString();
                        }
                        else if (week == Week.WeekThree)
                        {
                            rep = Mainlift.Reps.WeekThree.setThree.ToString();
                        }
                        else if (week == Week.WeekFour)
                        {
                            rep = Mainlift.Reps.WeekFour.setThree.ToString();
                        }
                        break;
                }

                double roundedWeight = Math.Round(entry.Key / 5.0) * 5;

                setString += "  " + roundedWeight + " x " + rep + ": " + FormatPlates(entry.Value);
                setString += Environment.NewLine;

                set++;
            }

            return setString;
        }

        private static string FormatWarmup(Dictionary<double, List<double>> warmupDictionary)
        {
            string warmupString = "Warm-up" + Environment.NewLine;

            int set = 1;
            foreach (KeyValuePair<double, List<double>> entry in warmupDictionary)
            {
                string rep = string.Empty;

                switch (set)
                {
                    case 1:
                        rep = Mainlift.Reps.Warmup.setOne.ToString();
                        break;
                    case 2:
                        rep = Mainlift.Reps.Warmup.setTwo.ToString();
                        break;
                    case 3:
                        rep = Mainlift.Reps.Warmup.setThree.ToString();
                        break;
                }

                double roundedWeight = Math.Round(entry.Key / 5.0) * 5;

                warmupString += "  " + roundedWeight + " x " + rep + ": " + FormatPlates(entry.Value);
                warmupString += Environment.NewLine;

                set++;
            }

            return warmupString; 
        }

        private static string FormatPlates(List<double> plates)
        {
            string plateString = string.Empty;

            foreach(double plate in plates)
            {
                plateString += "(" + plate + ")";
            }

            return plateString; 
        }
    }

    class Excercises
    {
        List<Exercise> Deadlift = new List<Exercise>() { Exercise.Deadlift, Exercise.FrontSquat };
        List<Exercise> Bench = new List<Exercise>() { Exercise.Bench, Exercise.InclineBench };
        List<Exercise> Squat = new List<Exercise>() { Exercise.Squat, Exercise.StraightDeadlift };
        List<Exercise> Overhead = new List<Exercise>() { Exercise.Overhead, Exercise.CloseGripBench };
    }

    public enum Exercise
    {
        Deadlift,
        Bench,
        Squat,
        Overhead,
        InclineBench,
        StraightDeadlift,
        FrontSquat,
        CloseGripBench
    }

    enum Week
    {
        WeekOne,
        WeekTwo,
        WeekThree,
        WeekFour
    }

    public class Maxes
    {
        public Maxes(double deadlift, double bench, double squat, double overhead, double inclineBench, double straightDeadlift, double frontSquat, double closeGripBench, Lifter lifter)
        {
            Person = lifter;

            Deadlift = deadlift;
            Bench = bench;
            Squat = squat;
            Overhead = overhead;
            InclineBench = inclineBench;
            StraightDeadlift = straightDeadlift;
            FrontSquat = frontSquat;
            CloseGripBench = closeGripBench;
        }

        public double Deadlift;

        public double Bench;

        public double Squat;

        public double Overhead;

        public double InclineBench;

        public double StraightDeadlift;

        public double FrontSquat;

        public double CloseGripBench;

        public Lifter Person; 
    }

    public class Mainlift
    {
        public struct Percentages
        {
            public struct WeekOne
            {
                public const double setOne = .65;
                public const double setTwo = .75;
                public const double setThree = .85;
            }

            public struct WeekTwo
            {
                public const double setOne = .70;
                public const double setTwo = .80;
                public const double setThree = .90;
            }

            public struct WeekThree
            {
                public const double setOne = .75;
                public const double setTwo = .85;
                public const double setThree = .95;
            }


            public struct WeekFour
            {
                public const double setOne = .40;
                public const double setTwo = .50;
                public const double setThree = .60;
            }

            public struct Warmup
            {
                public const double setOne = .40;
                public const double setTwo = .50;
                public const double setThree = .60;
            }
        }

        public struct Reps
        {
            public struct WeekOne
            {
                public const int setOne = 5;
                public const int setTwo = 5;
                public const int setThree = 5;
            }

            public struct WeekTwo
            {
                public const int setOne = 3;
                public const int setTwo = 3;
                public const int setThree = 3;
            }

            public struct WeekThree
            {
                public const int setOne = 5;
                public const int setTwo = 3;
                public const int setThree = 1;
            }


            public struct WeekFour
            {
                public const int setOne = 5;
                public const int setTwo = 5;
                public const int setThree = 5;
            }

            public struct Warmup
            {
                public const int setOne = 5;
                public const int setTwo = 5;
                public const int setThree = 3;
            }
        }
    }

    public class AssistanceLift
    {
        public struct Percentages
        {
            public struct WeekOne
            {
                public const double setOne = .50;
                public const double setTwo = .60;
                public const double setThree = .70;
            }

            public struct WeekTwo
            {
                public const double setOne = .60;
                public const double setTwo = .70;
                public const double setThree = .80;
            }

            public struct WeekThree
            {
                public const double setOne = .65;
                public const double setTwo = .75;
                public const double setThree = .85;
            }


            public struct WeekFour
            {
                public const double setOne = .40;
                public const double setTwo = .50;
                public const double setThree = .60;
            }
        }

        public struct Reps
        {
            public struct WeekOne
            {
                public const int setOne = 10;
                public const int setTwo = 10;
                public const int setThree = 10;
            }

            public struct WeekTwo
            {
                public const int setOne = 8;
                public const int setTwo = 8;
                public const int setThree = 6;
            }

            public struct WeekThree
            {
                public const int setOne = 5;
                public const int setTwo = 5;
                public const int setThree = 5;
            }


            public struct WeekFour
            {
                public const int setOne = 5;
                public const int setTwo = 5;
                public const int setThree = 5;
            }

            public struct Warmup
            {
                public const int setOne = 5;
                public const int setTwo = 5;
                public const int setThree = 3;
            }
        }
    }

    public enum Lifter
    {
        Micah,
        Matt
    };

    public class Plates
    {
        static List<double> AvailablePlates = new List<double>() { 45, 25, 10, 5, 2.5, 0};

        const double BarWeight = 45;
        const double OnePlate = 135;

        /// <summary>
        /// Input should be a multiple of 5 if possible. Otherwise will round up or down to closest multiple of 5.
        /// </summary>
        /// <param name="targetWeight"></param>
        /// <returns>List of plates to add to each side.</returns>
        public static List<double> SelectPlates(double targetWeight, Exercise exercise)
        {
            List<double> selectedPlates = new List<double>();

            if (targetWeight < BarWeight)
            {
                if (exercise == Exercise.Deadlift)
                {
                    selectedPlates.Add(45);
                }
                else
                {
                    selectedPlates.Add(0);
                }
            }
            else if ((exercise == Exercise.Deadlift) && targetWeight < OnePlate)
            {
                selectedPlates.Add(45);
            }
            else
            {
                // No rounding necessary
                if (targetWeight % 5 == 0)
                {
                    targetWeight = targetWeight - 45;
                }
                else
                {
                    double roundedWeight = Math.Round(targetWeight / 5.0) * 5;

                    targetWeight = roundedWeight - 45;
                }

                double temporaryWeight = targetWeight;

                foreach (double plate in AvailablePlates)
                {
                    while (temporaryWeight / plate >= 2)
                    {
                        temporaryWeight -= (plate * 2);
                        selectedPlates.Add(plate);
                    }
                }
            }

            return selectedPlates;
        }
    }
}
