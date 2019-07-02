using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraduationTracker.Helper;
using GraduationTracker.Models;
using GraduationTracker.Interfaces;

namespace GraduationTracker
{
    public class GraduationTracker
    {
        ILogger _logger = new FileLogger();
        public Tuple<bool, STANDING>  HasGraduated(Diploma diploma, Student student)
        {
            var standing = STANDING.None;
            try
            {
                var credits = 0;
                var average = 0;
                IRepository repo = new Repository();

                UpdateCredits(diploma, student, ref credits, ref average, repo);

                average = average / student.Courses.Length;

                standing = GetStandingForStudent(average);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StudentStanding(standing);
        }

        private Tuple<bool, STANDING> StudentStanding(STANDING standing)
        {
            try
            {
                switch (standing)
                {
                    case STANDING.Remedial:
                        return new Tuple<bool, STANDING>(false, standing);
                    case STANDING.Average:
                        return new Tuple<bool, STANDING>(true, standing);
                    case STANDING.SumaCumLaude:
                        return new Tuple<bool, STANDING>(true, standing);
                    case STANDING.MagnaCumLaude:
                        return new Tuple<bool, STANDING>(true, standing);
                    default:
                        return new Tuple<bool, STANDING>(false, standing);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return new Tuple<bool, STANDING>(false, standing);
        }

        private STANDING GetStandingForStudent(int average)
        {
            STANDING standing=STANDING.None;
            
            try
            {
                if (average < 50)
                    standing = STANDING.Remedial;
                else if (average < 80)
                    standing = STANDING.Average;
                else if (average < 95)
                    standing = STANDING.MagnaCumLaude;
                else
                    standing = STANDING.MagnaCumLaude;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            
            return standing;
        }

        private void UpdateCredits(Diploma diploma, Student student, ref int credits, ref int average, IRepository repo)
        {
            try
            {
                for (int i = 0; i < diploma.Requirements.Length; i++)
                {
                    for (int j = 0; j < student.Courses.Length; j++)
                    {
                        var requirement = repo.GetRequirement(diploma.Requirements[i]);

                        for (int k = 0; k < requirement.Courses.Length; k++)
                        {
                            if (requirement.Courses[k] == student.Courses[j].Id)
                            {
                                average += student.Courses[j].Mark;
                                if (student.Courses[j].Mark > requirement.MinimumMark)
                                {
                                    credits += requirement.Credits;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
        }
    }
}
