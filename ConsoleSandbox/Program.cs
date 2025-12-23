
using System.Diagnostics;
using ScrubJay.Functional;
using ScrubJay.Universal;
using ScrubJay.Universal.Tests;

TestRefStruct trs = new TestRefStruct(147, "TRJ");

TestRefStruct otherA = new(147, "OTHER_147");
TestRefStruct otherB = new(13, "OTHER_13");

bool eqA = Any.Equals(trs, otherA);
bool eqB = Any.Equals(trs, otherB);
int compA = Any.CompareTo(trs, otherA);
int compB = Any.CompareTo(trs, otherB);

string strA = Any.ToString(otherA);
string strB = Any.ToString(otherB);
int hcA = Any.GetHashCode(otherA);
int hcB = Any.GetHashCode(otherB);
Type typeA = Any.GetType(otherA);
Type typeB = Any.GetType(otherB);

Debugger.Break();


