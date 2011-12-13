using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib;
using NUnit.Framework;

namespace Tests {
  class Program {
    static void Main(string[] args) {
    }
  }

  [TestFixture]
  class CouplerTest {
    [Test]
    public void SimpleCoupling() {
      List<int> l1 = new List<int> {1, 5, 7}, l2 = new List<int> {3, 6, 9};
      var cpls = new Coupler().Couple(l1, l2).ToList();
      Assert.AreEqual(3, cpls.Count);
    }

    [Test]
    public void Unbalanced() {
       List<int> l1 = new List<int> {1, 60, 76}, l2 = new List<int> {3, 19, 22, 51, 56, 66, 67, 90};
      var cpls = new Coupler().Couple(l1, l2).ToList();
      Assert.AreEqual(2, cpls.Count);
      Assert.AreEqual(1, cpls[0].Begin);
      Assert.AreEqual(66, cpls[1].End);
    }

    [Test]
    public void NormalizeIsOk() {
      Assert.AreEqual("gelp", Reference.Normalize("2. gelp"));
      Assert.AreEqual("gelp", Reference.Normalize("gelp"));
    }
    
  }
}
