using AutoInsuranceQuotes.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace AutoInsuranceQuotes.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetQuote(string firstName, string lastName, string emailAddress, string dOB, string dUI, string coverage, int spdTicket, string carYear, string carMake, string carModel)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(dOB) || string.IsNullOrEmpty(dUI) || string.IsNullOrEmpty(coverage) || string.IsNullOrEmpty(carYear) || string.IsNullOrEmpty(carMake) || string.IsNullOrEmpty(carModel))
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                using (AutoInsuranceEntities db = new AutoInsuranceEntities())
                {
                    int birthYear = Convert.ToInt32(dOB);
                    DateTime Today = DateTime.Today;
                    int Year = Convert.ToInt32(Today.Year);
                    int age = Year - birthYear;
                    int strtquote = 50;
                    int cYear = Convert.ToInt32(carYear);
                    int quote;

                    if (age < 18)
                    {
                        strtquote += 100;
                    }
                    else if (age < 25 || age > 100)
                    {
                        strtquote += 100;
                    }

                    if (cYear < 2000 || cYear > 2015)
                    {
                        strtquote += 25;
                    }

                    if (carMake == "Porsche" || carMake == "porsche" && carModel == "911 Carrera")
                    {
                        if (carMake == "Porsche" || carMake == "porsche")
                        {
                            strtquote += 25;
                        }
                        strtquote += 50;
                    }

                    if (spdTicket > 0)
                    {
                        strtquote += 10 * spdTicket;
                    }

                    if (dUI == "Yes")
                    {
                        strtquote = Convert.ToInt32(strtquote * 1.25);
                    }

                    if (coverage == "full coverage" || coverage == "Full Coverage")
                    {
                        strtquote = Convert.ToInt32(strtquote * 1.5);
                    }
                    quote = strtquote;

                    var quotes = new Quote();
                    quotes.FirstName = firstName;
                    quotes.LastName = lastName;
                    quotes.EmailAddress = emailAddress;
                    quotes.DOB = dOB;
                    quotes.DUI = dUI;
                    quotes.Coverage = coverage;
                    quotes.SpdTicket = spdTicket;
                    quotes.CarYear = carYear;
                    quotes.CarMake = carMake;
                    quotes.CarModel = carModel;
                    quotes.Quote1 = Convert.ToString(quote);

                    db.Quotes.Add(quotes);
                    db.SaveChanges();

                }
                
                    return View("Quote");
            }
        }

        public ActionResult Admin()
        {
            using (AutoInsuranceEntities db = new AutoInsuranceEntities())
            {
                var quotes = db.Quotes;
                var adminQuotesVm = new List<AdminVm>();
                foreach (var user in quotes)
                {
                    var adminquote = new AdminVm();
                    adminquote.FirstName = user.FirstName;
                    adminquote.LastName = user.LastName;
                    adminquote.EmailAddress = user.EmailAddress;
                    adminquote.Quote = user.Quote1;
                    adminQuotesVm.Add(adminquote);
                }
                return View(adminQuotesVm);
            }               
        }
    }
}