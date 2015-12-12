﻿/*

 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyBeanVMAssemblerCLI.Parsing
{
	/// <summary>
	/// Description of ASMParse.
	/// </summary>
	public class ASMParse
	{
		//static short offset = 0;
		static Bictionary<string, short[]> opcodes = new Bictionary<string, short[]>()
		{
			{ "lbl:", new short[]{ 0x005f, 0x0000} },
			{ "nop", new short[]{ 0x0000, 0x000f} },
			{ "dmpreg", new short[]{0x0000, 0x1100} }, //dump registers (debugging)
			{ "dmpmem", new short[]{0x0000, 0x1101} }, //dump memory (debugging)
			{ "lda", new short[]{0x0000, 0x1110} }, //load into A register
			{ "mov", new short[]{0x0000, 0x1111} }, //mov value to location
			{ "push", new short[]{0x0000, 0x1112} }, //push value
			{ "pop", new short[]{0x0000, 0x1113} }, //pop value
			{ "bcall", new short[]{0x0000, 0x1114} }, //call built-in code
		};
		static Dictionary<short, int> RegisterIds = new Dictionary<short, int>()
		{
			{0x1000,1},
			{0x1001,2},
			{0x1002,3},
			{0x1003,4},
		};

		public static short[] lbl()
		{
			return opcodes["lbl:"]; //label
		}
		public static short[] nop()
		{
			return opcodes["nop"]; //nop			
		}
		public static short[] s2opc(string s)
		{
			//0x0000 - instruction
			return opcodes[s];
			/*
			switch (s)
			{
				
				case "lda":
					return new short[]{0x0000 , 0x1110};
				case "mov":
					return new short[]{0x0000 , 0x1111};
				default:
					return new short[]{0x0000 , 0x000f};
			}
			*/
		}
		public static string opc2s(short[] opc)
		{
			return opcodes[opc];
		}
		public static int by_r_type(short[] by)
		{
			if (by[0] == 0x0001) //register
			{
				return 1; //register
			}
			if (by[0] == 0x0002) //num literal
			{
				return 0; //numerical literal
			}
			if (by[0] == 0x0003) //memory address
			{
				return 2; //memory address
			}
			if (by[0] == 0x000f) //name
			{
				return 3; //name
			}
			
			return -1; //unknown type
		}
		public static  short lit2sh(short[] lit) //literal to short
		{
			return lit[1];
		}
		public static int rlit2sh(short[] lit) //register literal to register id
		{
			return RegisterIds[lit[1]];
		}
		public static short[] r2by(string s)
		{
			switch (s)
			{
				//0x0001 - register
				//0x0002 - constant
				//REGISTERS:
				//0x1000 - A
				case "a":
					return new short[] {0x0001, 0x1000};
				case "b":
					return new short[] {0x0001, 0x1001};
				case "t":
					return new short[] {0x0001, 0x1002};
				case "x":
					return new short[] {0x0001, 0x1003};
				default:
					bool memloc = s.StartsWith("$"); //$ to access RAM
					bool name = s.StartsWith("*");
					if (!memloc && !name)
					{
						short d = 0x0000;
						bool x = short.TryParse(s, out d);
						return new short[]{ 0x0002, d};
					}
					else if (memloc && !name)
					{
						string s1 = s.Substring(1);
						short d = 0x0000;
						bool x = short.TryParse(s1, out d);
						return new short[]{ 0x0003, d};
					}
					else if (name) //0x000f - name type
					{
						string s1 = s.Substring(1);
						short d = 0x0000;
						bool x = short.TryParse(s1, out d);
						return new short[]{ 0x000f, d};
					}
					return opcodes["nop"];
			}
		}
	}
	public class Bictionary<T1, T2> : Dictionary<T1, T2>
	{
	    public T1 this[T2 index]
	    {
	        get
	        {
	            if(!this.Any(x => x.Value.Equals(index)))
	               throw new System.Collections.Generic.KeyNotFoundException();
	            return this.First(x => x.Value.Equals(index)).Key;
	        }
	    }
	}
}