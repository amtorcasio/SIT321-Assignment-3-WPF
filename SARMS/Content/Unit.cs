﻿using System;
using System.Collections.Generic;

namespace SARMS.Content
{
    public class Unit
    {
        #region Private Fields
        private int _id;
        private string _code;
        private DateTime _year;
        private int _trimester;
        private int _totalLectures;
        private int _totalPracticals;
        #endregion

        #region Getters and Setters
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        public DateTime Year
        {
            get { return _year; }
            set { _year = value; }
        }
        public int Trimester
        {
            get { return _trimester; }
            set { _trimester = value; }
        }
        public int TotalLectures
        {
            get { return _totalLectures; }
            set { _totalLectures = value; }
        }
        public int TotalPracticals
        {
            get { return _totalPracticals; }
            set { _totalPracticals = value; }
        }
        #endregion

        public List<Assessment> Assessments;

        // constructor
        public Unit(int id, string code, DateTime year, int trimester, int totalLectures, int totalPracticals, List<Assessment> assessments)
        {
            _id = id;
            _code = code;
            _year = year;
            _trimester = trimester;
            _totalLectures = totalLectures;
            _totalPracticals = totalPracticals;
            Assessments = assessments;
        }
    }
}
