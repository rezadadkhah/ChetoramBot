using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models;

namespace Business.Helpers
{
    public static class Consts
    {
        public static List<Survey> Surveys => new List<Survey>()
        {
            new Survey() {Id = 1, PersianTitle = "مهربونه؟"},
            new Survey() {Id = 2, PersianTitle = "خوش اخلاقه؟"},
            new Survey() {Id = 3, PersianTitle = "باهوشه؟"},
            new Survey() {Id = 4, PersianTitle = "خوشگله؟"},
            new Survey() {Id = 5, PersianTitle = "دوست داری بهش نزدیک تر بشی؟"}
        };
    }
}
