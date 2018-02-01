namespace GeneticEvolution
{
    public class Phenotype<TPhenotype>
    {
        public TPhenotype Data;
        public float Fitness;

        public Phenotype(TPhenotype phenotype)
        {
            Data = phenotype;
        }
    }
}