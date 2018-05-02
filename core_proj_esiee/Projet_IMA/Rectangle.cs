using System;
using System.Collections.Generic;

namespace Projet_IMA
{
  class Rectangle : Objet
  {
        private V3 a;
        private V3 b;
        private V3 c;
        private V3 normale;

        public Rectangle(V3 a, V3 b, V3 c, Texture texture) : base(texture)
        {
          this.a = a;
          this.b = b;
          this.c = c;

          V3 vecteurAB = this.b - this.a;
          V3 vecteurAC = this.c - this.a;
          this.normale = vecteurAB ^ vecteurAC;
          this.normale.Normalize();
        }

        public Rectangle(V3 a, V3 b, V3 c, Couleur couleur) : base(couleur)
        {
          this.a = a;
          this.b = b;
          this.c = c;

          V3 vecteurAB = this.b - this.a;
          V3 vecteurAC = this.c - this.a;
          this.normale = vecteurAB ^ vecteurAC;
          this.normale.Normalize();
        }

        public override V3 Calculer(float u, float v)
        {
          V3 vecteurAB = this.b - this.a;
          V3 vecteurAC = this.c - this.a;
          return new V3(this.a + u * vecteurAB + v * vecteurAC);
        }

        public override void Draw(V3 camera, int[,] zbuffer, List<Lampe> lampes)
        {
            float step = 0.01f;
            for (float u = 0; u <= 1; u += step)
            {
              for (float v = 0; v <= 1; v += step)
              {
                V3 currentPoint;
                currentPoint = Calculer(u, v);

                int x = (int) (currentPoint.x);
                int y = (int) (currentPoint.y);
                int z = (int) (currentPoint.z);

                if (y < zbuffer[z, x])
                {
                  Couleur couleurAffichee;
                  V3 bumpNormale;

                  if (this.texture != null)
                  {
                    this.couleur = this.texture.LireCouleur(u, v);

                    bumpNormale = BumpNormale(u, v);
                    bumpNormale.Normalize();
                    this.normale = bumpNormale;
                  }

                  couleurAffichee = LampesEffectsOnCouleur(lampes, this.couleur, camera);

                  BitmapEcran.DrawPixel(x, z, couleurAffichee);

                  zbuffer[z, x] = y;
                }
              }
            }
        }
    }
}
