using System.Collections.Generic;
using System.Linq;
using GeneticEvolution;
using UnityEngine;

namespace Tests.GeneticAlgorithms
{
    public class FunctionExtremePointsGeneticAlgorithm2 : GeneticEvolution<Vector2>
    {
        public FunctionExtremePointsGeneticAlgorithm2(List<Vector2> zeroGeneration, int maxGenerations, float solutionThreshold) : base(zeroGeneration, maxGenerations, solutionThreshold)
        {
        }
        
        protected override List<Phenotype<Vector2>> DoSelections(List<Phenotype<Vector2>> generation)
        {
            var outList = new List<Phenotype<Vector2>>();
            var sortedGeneration = generation.OrderBy(p => p.Fitness).ToList();
            outList.Add(sortedGeneration[0]);
            outList.Add(new Phenotype<Vector2>(new Vector2(sortedGeneration[1].Data.x, sortedGeneration[0].Data.y)));
            outList.Add(new Phenotype<Vector2>(new Vector2(sortedGeneration[0].Data.x, sortedGeneration[2].Data.y)));
            return outList;
        }

        protected override List<Phenotype<Vector2>> DoMutations(List<Phenotype<Vector2>> generation)
        {
            var outList = new List<Phenotype<Vector2>>();
            var sortedGeneration = generation.OrderBy(p => p.Fitness).ToList();

            var vector2 = sortedGeneration.Last().Data;

            if (CurrentGeneration % 5 == 0)
            {
                vector2.x *= -1;
                vector2.y *= -1;
            }
            
            outList.Add(new Phenotype<Vector2>(new Vector2(sortedGeneration[1].Data.x + CurrentGeneration % 2, sortedGeneration[0].Data.y + (CurrentGeneration + 1) % 2)));
            
            return outList;
        }

        protected override float GetFitness(Vector2 phenotype)
        {
            return -(phenotype.x / (Mathf.Pow(phenotype.x, 2)  + 2 * Mathf.Pow(phenotype.y, 2) + 1f));
        }
    }
}