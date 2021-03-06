﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SARMS.Content
{
    public class Unit : IEquatable<Unit>
    {
        #region Private Fields
        private long _id;
        private string _name;
        private string _code;
        private int _year;
        private int _trimester;
        private int _totalLectures;
        private int _totalPracticals;
        #endregion

        #region Getters and Setters
        public long ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        public int Year
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
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        #endregion

        public ObservableCollection<Assessment> Assessments;

        public string AssessmentCount
        {
            get { return (Assessments != null) ? Assessments.Count.ToString() : "0"; }
            private set { }
        }

        // constructor
        public Unit(long id, string name, string code, int year, int trimester, int totalLectures, int totalPracticals)
        {
            _id = id;
            _name = name;
            _code = code;
            _year = year;
            _trimester = trimester;
            _totalLectures = totalLectures;
            _totalPracticals = totalPracticals;
        }

        public override string ToString()
        {
            return Code + ": " + Name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Unit);
        }

        public bool Equals (Unit other)
        {
            if (other == null) return false;
            if (other.ID == ID) return true;
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
