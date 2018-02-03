using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticEvolution
{
    public abstract class GeneticEvolution<TPhenotype>
    {
        protected readonly List<List<Phenotype<TPhenotype>>> Generations = new List<List<Phenotype<TPhenotype>>>();
        protected int CurrentGeneration;

        private readonly int _maxGenerations;
        private readonly float _solutionThreshold;

        private Phenotype<TPhenotype> _bestSolution;

        protected GeneticEvolution(List<TPhenotype> zeroGeneration, int maxGenerations, float solutionThreshold)
        {
            _maxGenerations = maxGenerations;
            _solutionThreshold = solutionThreshold;

            Generations.Add(zeroGeneration.Select(p => new Phenotype<TPhenotype>(p)).ToList());
            _bestSolution = Generations[0].First();
        }

        public Phenotype<TPhenotype> Run()
        {
            while (!CheckEndEvolution())
            {
                ProcessCurrentGeneration();
            }            
            
            return _bestSolution;
        }

        private void ProcessCurrentGeneration()
        {
            var newGeneration = new List<Phenotype<TPhenotype>>();
            
            newGeneration.AddRange(DoSelections(Generations[CurrentGeneration]));
            newGeneration.AddRange(DoMutations(Generations[CurrentGeneration]));
            
            CalculateFitness(newGeneration);

            foreach (var phenotype in newGeneration)
            {
                if (phenotype.Fitness <= _bestSolution.Fitness)
                {
                    _bestSolution = phenotype;
                }
            }
            
            Generations.Add(newGeneration);           
            CurrentGeneration++;
        }

        protected abstract List<Phenotype<TPhenotype>> DoSelections(List<Phenotype<TPhenotype>> generation);

        protected abstract List<Phenotype<TPhenotype>> DoMutations(List<Phenotype<TPhenotype>> generation);

        protected abstract float GetFitness(TPhenotype phenotype);

        private bool CheckEndEvolution()
        {
            return CurrentGeneration > _maxGenerations || _bestSolution.Fitness <= _solutionThreshold;
        }

        private void CalculateFitness(List<Phenotype<TPhenotype>> generation)
        {
            foreach (var phenotype in generation)
            {
                phenotype.Fitness = GetFitness(phenotype.Data);
            }
        }
        
        
    }
}