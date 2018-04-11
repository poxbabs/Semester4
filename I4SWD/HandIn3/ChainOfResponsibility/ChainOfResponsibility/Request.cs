﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainOfResponsibility
{
    public enum RequestType
    {
        Formal,
        Regular,
        Urgent,
        Critical
    }
	public class Request
	{
		public Request(RequestType type, string name)
		{
		    RequestType = type;
			Name = name;
		}

		public RequestType RequestType { get; private set; }
		public string Name { get; private set; }
	}
}