using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace CuSum_Analysis
{
    
    class CuSumParameters
    {
        public decimal alpha;
        public decimal beta;
        public decimal p0;
        public decimal p1;
        ////
        public decimal maxAccLevelOfError;
        public decimal P;
        public decimal Q;
        public decimal a;
        public decimal b;
        public decimal s;
        public decimal h0;
        public decimal h1;
        //private decimal[] currentVal = new decimal[4];
        public CuSumParameters(decimal alpha, decimal beta, decimal p0, decimal p1)
        {
            try
            {
                this.alpha = alpha;
                this.beta = beta;
                this.p0 = p0;
                this.p1 = p1;
                //Console.WriteLine("alpha: {0}, beta: {1}, p0: {2}, p1: {3}", alpha, beta, p0, p1);
                if (p1 <= p0) { return; }
                
                this.maxAccLevelOfError = this.p1 - this.p0;
                
                    this.P = (decimal)Math.Log((double)(this.p1 / this.p0));
                    this.Q = (decimal)Math.Log((double)((1 - this.p0) / (1 - this.p1)));
                    this.a = (decimal)Math.Log((double)((1 - this.beta) / (this.alpha)));
                    this.b = (decimal)Math.Log((double)((1 - this.alpha) / (this.beta)));
                    this.s = this.Q / (this.P + this.Q);
                    this.h0 = this.b / (this.P + this.Q);
                    this.h1 = this.a / (this.P + this.Q);
                Console.WriteLine("alpha: {0}, beta: {1}, p0: {2}, p1: {3}", alpha, beta, p0, p1);
                Console.WriteLine("p: {0}, q: {1}, a: {2}, b: {3}, s: {4}, h0: {5}, h1: {6}", P, Q, a, b,s,h0,h1);


            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "CuSum Class", MessageBoxButtons.OK);
                
            }
        }
    }
}
