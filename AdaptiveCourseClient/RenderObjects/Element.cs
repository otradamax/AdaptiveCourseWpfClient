﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdaptiveCourseClient.RenderObjects
{
    public abstract class Element
    {
        protected Canvas? _canvas;

        public Element(Canvas? canvas)
        {
            _canvas = canvas;
        }

        public abstract void MakeConnection(ConnectionLine connectionLine);
    }
}