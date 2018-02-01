using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticEvolution
{
    public class GeneticEvolution<TPhenotype>
    {
        private List<List<Phenotype<TPhenotype>>> _generations = new List<List<Phenotype<TPhenotype>>>();
        private int _currentGeneration;

        private Action<List<Phenotype<TPhenotype>>, int, List<TPhenotype>> _selection;
        private Action<List<Phenotype<TPhenotype>>, int, List<TPhenotype>> _mutation;
        private Func<TPhenotype, float> _fitnessFunction;

        private int _maxGenerations;
        private float _solutionThreshold;

        private Phenotype<TPhenotype> _bestSolution;
        
        public void Create(List<TPhenotype> zeroGeneration, Func<TPhenotype, float> fitnessFunction,
            Action<List<Phenotype<TPhenotype>>, int, List<TPhenotype>> selection,
            Action<List<Phenotype<TPhenotype>>, int, List<TPhenotype>> mutation, int maxGenerations, float solutionThreshold)
        {
            if (_fitnessFunction == null)
            {
                throw new ArgumentException("Fitness function must be not null");
            }
            
            _fitnessFunction = fitnessFunction;
            _selection = selection;
            _mutation = mutation;

            _maxGenerations = maxGenerations;
            _solutionThreshold = solutionThreshold;
            
            _generations.Add(zeroGeneration.Select(p => new Phenotype<TPhenotype>(p)).ToList());
        }

        public Phenotype<TPhenotype> Run()
        {
            while (!CheckEndEvolution())
            {
                ProcessCurrentGeneration();
                _currentGeneration++;    
            }
            
            return _bestSolution;
        }

        private void ProcessCurrentGeneration()
        {
            
        }

        private bool CheckEndEvolution()
        {
            return _currentGeneration > _maxGenerations || _bestSolution.Fitness <= _solutionThreshold;
        }

        private void CalculateFitness()
        {
            var currentGeneration = _generations[_currentGeneration];
            foreach (var phenotype in currentGeneration)
            {
                phenotype.Fitness = _fitnessFunction.Invoke(phenotype.Data);
            }
        }
        
        
    }
}