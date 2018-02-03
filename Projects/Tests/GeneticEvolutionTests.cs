using System;
using System.Collections.Generic;
using NUnit.Framework;
using Tests.GeneticAlgorithms;
using UnityEngine;

namespace Tests
{
    [TestFixture]
    public class GeneticEvolutionTests
    {
        [Test]
        public void FunctionExtremeTest1()
        {
            var genEvolution = new FunctionExtremePointsGeneticAlgorithm1(new List<Vector3>()
            {
                Vector3.one,
                Vector3.zero,
                Vector3.forward
            }, 50, float.MinValue);
            var result = genEvolution.Run();
            Console.WriteLine(result);
        }
    }
}