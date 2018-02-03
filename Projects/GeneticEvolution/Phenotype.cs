namespace GeneticEvolution
{
    public class Phenotype<TPhenotype>
    {
        public readonly TPhenotype Data;
        public float Fitness = float.MaxValue;

        public Phenotype(TPhenotype phenotype)
        {
            Data = phenotype;
        }

        public override string ToString()
        {
            return $"{Data} : {Fitness}";
        }
    }
}