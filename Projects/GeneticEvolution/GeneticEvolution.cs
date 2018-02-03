using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticEvolution
{
    public abstract class GeneticEvolution<TPhenotype>
    {
        protected readonly List<List<Phenotype<TPhenotype>>> Generations = new List<List<Phenotype<TPhenotype>>>();
        protected int CurrentGeneration;

        private int _maxGenerations;
        private float _solutionThreshold;

        private Phenotype<TPhenotype> _bestSolution;
        
        public void Create(List<TPhenotype> zeroGeneration, int maxGenerations, float solutionThreshold)
        {
            _maxGenerations = maxGenerations;
            _solutionThreshold = solutionThreshold;
            
            Generations.Add(zeroGeneration.Select(p => new Phenotype<TPhenotype>(p)).ToList());
        }

        public Phenotype<TPhenotype> Run()
        {
            while (!CheckEndEvolution())
            {
                ProcessCurrentGeneration();
                CurrentGeneration++;    
            }
            
            return _bestSolution;
        }

        private void ProcessCurrentGeneration()
        {
            CalculateFitness();
            
            var newGeneration = new List<Phenotype<TPhenotype>>();
            
            newGeneration.AddRange(DoSelections());
            newGeneration.AddRange(DoMutations());

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

        protected abstract List<Phenotype<TPhenotype>> DoSelections();

        protected abstract List<Phenotype<TPhenotype>> DoMutations();

        protected abstract float GetFitness(TPhenotype phenotype);

        private bool CheckEndEvolution()
        {
            return CurrentGeneration > _maxGenerations || _bestSolution.Fitness <= _solutionThreshold;
        }

        private void CalculateFitness()
        {
            var currentGeneration = Generations[CurrentGeneration];
            foreach (var phenotype in currentGeneration)
            {
                phenotype.Fitness = GetFitness(phenotype.Data);
            }
        }
        
        
    }
}