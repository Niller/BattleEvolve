using System.Collections.Generic;
using GeneticEvolution;
using UnityEngine;

namespace Tests.GeneticAlgorithms
{
    public class FunctionExtremePointsGeneticAlgorithm1 : GeneticEvolution<Vector3>
    {
        public FunctionExtremePointsGeneticAlgorithm1(List<Vector3> zeroGeneration, int maxGenerations, float solutionThreshold) : base(zeroGeneration, maxGenerations, solutionThreshold)
        {
        }
        
        protected override List<Phenotype<Vector3>> DoSelections(List<Phenotype<Vector3>> generation)
        {
            return new List<Phenotype<Vector3>>();
        }

        protected override List<Phenotype<Vector3>> DoMutations(List<Phenotype<Vector3>> generation)
        {
            var newPhenotypes = new List<Phenotype<Vector3>>();
            foreach (var phenotype in generation)
            {
                var newPhenotype = new Phenotype<Vector3>(phenotype.Data + new Vector3(CurrentGeneration%3, (CurrentGeneration+1)%3, (CurrentGeneration+2)%3));
                newPhenotypes.Add(newPhenotype);
            }

            return newPhenotypes;
        }

        protected override float GetFitness(Vector3 phenotype)
        {
            return phenotype.x + phenotype.y - phenotype.z;
        }
    }
}