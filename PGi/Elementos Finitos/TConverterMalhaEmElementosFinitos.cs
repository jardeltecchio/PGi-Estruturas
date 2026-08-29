using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{ 
    public class TConverterMalhaEmElementosFinitos
    {
       List<Celula> celulas;
       public TConverterMalhaEmElementosFinitos(List<Celula> _celulas)
       {
          celulas = _celulas;
       }

        public List<Q4_Secao> quad4;
        public List<T6_Secao> tris6;
        public List<TNoMEF> nosElementos;
        List<Celula> cels_tri;
        TNoMEF LocalizaNoElemento(double xi, double yi)
        {
            for (int jk = 0; jk < nosElementos.Count; jk++)
                if ((Geom.Iguais(xi, nosElementos[jk].x, 0.01f) && Geom.Iguais(yi, nosElementos[jk].y, 0.01f)))
                    return nosElementos[jk];

            return null;
        }
        TNoMEF AtribuiNo(double x, double y)
        {
            TNoMEF no1 = LocalizaNoElemento(x, y);
            if (no1 == null)
            {
                no1 = new TNoMEF(x, y, 0);
                no1.numero = nosElementos.Count + 1;
                nosElementos.Add(no1);
            }

            return no1;
        }

        public void Gerar()
        {
            tris6 = new List<T6_Secao>();
            quad4 = new List<Q4_Secao>();
            nosElementos = new List<TNoMEF>();

            TNoMEF no1, no2, no3, no4, no5, no6, no7, no8, no9;
            List<Celula> celsQuad = new List<Celula>();

            //insere t6;
            cels_tri = celulas.FindAll(o =>/*o.id == 1064 &&*/ !o.avulso /*&& !o.mesclou */&& (o.N1.aresta || o.N2.aresta || o.N3.aresta || o.N4.aresta) && (Geom.Iguais(o.angulo_n1, 180, 1) || Geom.Iguais(o.angulo_n2, 180, 1) || Geom.Iguais(o.angulo_n3, 180, 1) || Geom.Iguais(o.angulo_n4, 180, 1)));
            Celula celtri;
            for (int i = 0; i < cels_tri.Count; i++)
            {
                celtri = cels_tri[i];

                int nn1 = 0;
                for (int j = 0; j < celtri.nos.Count; j++)
                {
                    if (Geom.Iguais(celtri.angulos_internos[j], 180, 0.1))
                    {
                        NoCelula nn = celtri.retNoOposto(j, ref nn1);
                        break;
                    }
                }

                int nn2 = 0, nn3 = 0;

                if (nn1 == 0)
                {
                    nn2 = 1;
                    nn3 = 3;
                }
                else
                if (nn1 == 1)
                {
                    nn2 = 2;
                    nn3 = 0;
                }
                else
                if (nn1 == 2)
                {
                    nn2 = 3;
                    nn3 = 1;
                }
                else
                if (nn1 == 3)
                {
                    nn2 = 0;
                    nn3 = 2;
                }

                no1 = AtribuiNo(celtri.nos[nn1].x, celtri.nos[nn1].y);
                no2 = AtribuiNo(celtri.nos[nn2].x, celtri.nos[nn2].y);
                no3 = AtribuiNo(celtri.nos[nn3].x, celtri.nos[nn3].y);

                no4 = AtribuiNo(((no1 + no2) / 2).x, ((no1 + no2) / 2).y);
                no5 = AtribuiNo(((no2 + no3) / 2).x, ((no2 + no3) / 2).y);
                no6 = AtribuiNo(((no3 + no1) / 2).x, ((no3 + no1) / 2).y);

                tris6.Add(new T6_Secao(no1, no2, no3, no4, no5, no6, tris6.Count));
            }

                //insere q4;
                celsQuad = celulas.FindAll(o =>/*o.id == 1064 &&*/ !o.avulso /*&& !o.mesclou */
                && (!Geom.Iguais(o.angulo_n1, 180, 1)
                && !Geom.Iguais(o.angulo_n2, 180, 1)
                && !Geom.Iguais(o.angulo_n3, 180, 1)
                && !Geom.Iguais(o.angulo_n4, 180, 1)));

                Celula celQuad;
                List<TNoMEF> nos_ = new List<TNoMEF>();

                for (int i = 0; i < celsQuad.Count; i++)
                {
                    celQuad = celsQuad[i];
                    TNoMEF n1, n2, n3, n4;

                    n1 = new TNoMEF(celQuad.nos[0].x, celQuad.nos[0].y, 0);
                    n2 = new TNoMEF(celQuad.nos[1].x, celQuad.nos[1].y, 0);
                    n3 = new TNoMEF(celQuad.nos[2].x, celQuad.nos[2].y, 0);
                    n4 = new TNoMEF(celQuad.nos[3].x, celQuad.nos[3].y, 0);

                    nos_.Clear();

                    nos_.Add(n1);
                    nos_.Add(n2);
                    nos_.Add(n3);
                    nos_.Add(n4);

                    no1 = AtribuiNo(nos_[0].x, nos_[0].y);
                    no2 = AtribuiNo(nos_[1].x, nos_[1].y);
                    no3 = AtribuiNo(nos_[2].x, nos_[2].y);
                    no4 = AtribuiNo(nos_[3].x, nos_[3].y);

                    Q4_Secao q4 = new Q4_Secao(no1, no2, no3, no4, quad4.Count);

                    quad4.Add(q4);
                }
            
        }
    }
}
