<template>
  <div class="page-container">
    <header class="page-hero">
      <div class="hero-overlay">
        <div class="hero-content">
          <h1 class="hero-title">Recetas Destacadas</h1>
          <p class="hero-subtitle">Las mejores recetas según nuestra comunidad</p>
        </div>
      </div>
    </header>

    <section class="content-section">
      <div class="container">
        <div class="filters-bar">
          <button @click="$router.push('/')" class="btn-inicio">
            <i class="pi pi-arrow-left"></i>
            Inicio
          </button>
          <div class="destacadas-info">
            <span class="badge-destacada">
              <i class="pi pi-star-fill"></i>
              Calificación 4+ estrellas
            </span>
          </div>
        </div>

        <div v-if="cargando" class="loading-state">
          <i class="pi pi-spin pi-spinner"></i>
          Cargando recetas destacadas...
        </div>

        <div v-else-if="recetas.length === 0" class="empty-state">
          <i class="pi pi-star"></i>
          <h3>Aún no hay recetas destacadas</h3>
          <p>Las recetas aparecerán aquí cuando reciban calificaciones altas de la comunidad.</p>
        </div>

        <div v-else class="recipes-grid">
          <div 
            v-for="receta in recetas" 
            :key="receta.id" 
            class="recipe-card destacada"
            @click="verReceta(receta.id)"
          >
            <div class="recipe-image">
              <div class="recipe-badge destacada-badge">
                <i class="pi pi-star-fill"></i>
                {{ receta.calificacionPromedio.toFixed(1) }}
              </div>
            </div>
            
            <div class="recipe-content">
              <h3 class="recipe-title">{{ receta.titulo }}</h3>
              <p class="recipe-desc">{{ receta.descripcion }}</p>
              
              <div class="recipe-meta">
                <span class="time">
                  <i class="pi pi-clock"></i>
                  {{ receta.tiempoPreparacion }} min
                </span>
                <span class="category">
                  <i class="pi pi-tag"></i>
                  {{ receta.categoriaNombre }}
                </span>
              </div>

              <div class="recipe-stats">
                <div class="rating-display">
                  <div class="stars">
                    <i 
                      v-for="n in 5" 
                      :key="n"
                      class="pi"
                      :class="n <= Math.round(receta.calificacionPromedio) ? 'pi-star-fill' : 'pi-star'"
                    ></i>
                  </div>
                  <span class="rating-value">{{ receta.calificacionPromedio.toFixed(1) }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import { recetaService } from '../services/recetaService'

export default {
  name: 'RecetasDestacadasView',
  data() {
    return {
      recetas: [],
      cargando: true
    }
  },
  async mounted() {
    await this.cargarRecetasDestacadas()
  },
  methods: {
    async cargarRecetasDestacadas() {
      try {
        this.cargando = true
        // Necesitarás agregar este método al recetaService
        this.recetas = await recetaService.obtenerDestacadas()
        console.log('✅ Recetas destacadas cargadas:', this.recetas)
      } catch (error) {
        console.error('❌ Error cargando recetas destacadas:', error)
        this.recetas = []
      } finally {
        this.cargando = false
      }
    },
    verReceta(id) {
      this.$router.push(`/receta/${id}`)
    }
  }
}
</script>

<style scoped>
/* ESTILOS BASE (igual que otras vistas) */
.page-container {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 20px;
}

.page-hero {
  height: 40vh;
  background: linear-gradient(rgba(0,0,0,0.4), rgba(0,0,0,0.4)), 
              url('https://images.unsplash.com/photo-1504674900247-0877df9cc836?ixlib=rb-4.0.3&auto=format&fit=crop&w=2070&q=80');
  background-size: cover;
  background-position: center;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  text-align: center;
}

.hero-content {
  max-width: 600px;
}

.hero-title {
  font-size: 3rem;
  font-weight: 300;
  margin-bottom: 1rem;
  letter-spacing: 2px;
}

.hero-subtitle {
  font-size: 1.3rem;
  opacity: 0.9;
  font-weight: 300;
}

.filters-bar {
  display: flex;
  gap: 2rem;
  margin-bottom: 2rem;
  align-items: center;
  margin-top: 2rem;
  justify-content: space-between;
}

.btn-inicio {
  background: transparent;
  border: 2px solid #e74c3c;
  color: #e74c3c;
  padding: 10px 20px;
  border-radius: 25px;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
  transition: all 0.3s ease;
  white-space: nowrap;
}

.btn-inicio:hover {
  background: #e74c3c;
  color: white;
}

.destacadas-info {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.badge-destacada {
  background: linear-gradient(135deg, #f39c12, #e67e22);
  color: white;
  padding: 8px 16px;
  border-radius: 20px;
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 8px;
}

/* ESTILOS ESPECIALES PARA RECETAS DESTACADAS */
.recipe-card.destacada {
  border: 2px solid #f39c12;
  box-shadow: 0 8px 25px rgba(243, 156, 18, 0.2);
}

.recipe-card.destacada:hover {
  transform: translateY(-8px);
  box-shadow: 0 15px 40px rgba(243, 156, 18, 0.3);
}

.destacada-badge {
  background: linear-gradient(135deg, #f39c12, #e67e22);
  color: white;
  padding: 6px 12px;
  border-radius: 15px;
  font-size: 0.9rem;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 5px;
  position: absolute;
  top: 1rem;
  right: 1rem;
}

.recipe-stats {
  margin-top: 1rem;
  padding-top: 1rem;
  border-top: 1px solid #eee;
}

.rating-display {
  display: flex;
  align-items: center;
  gap: 10px;
}

.rating-value {
  font-weight: 600;
  color: #f39c12;
  font-size: 1.1rem;
}

/* Grid y cards (igual que RecetasView) */
.recipes-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 2rem;
}

.recipe-card {
  background: white;
  border-radius: 10px;
  overflow: hidden;
  cursor: pointer;
  transition: all 0.3s ease;
  box-shadow: 0 5px 15px rgba(0,0,0,0.1);
  border: 1px solid #e9ecef;
}

.recipe-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 15px 40px rgba(0,0,0,0.15);
  border-color: #e74c3c;
}

.recipe-image {
  height: 150px;
  background: linear-gradient(rgba(0,0,0,0.3), rgba(0,0,0,0.3)), 
              url('https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=80');
  background-size: cover;
  background-position: center;
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
}

.recipe-content {
  padding: 1.5rem;
}

.recipe-title {
  font-size: 1.2rem;
  color: #2c3e50;
  margin-bottom: 0.8rem;
  font-weight: 500;
}

.recipe-desc {
  color: #7f8c8d;
  line-height: 1.5;
  margin-bottom: 1.2rem;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.recipe-meta {
  display: flex;
  justify-content: space-between;
  align-items: center;
  color: #95a5a6;
  font-size: 0.9rem;
  margin-bottom: 1rem;
}

.recipe-meta span {
  display: flex;
  align-items: center;
  gap: 5px;
}

.stars {
  color: #f39c12;
}

.loading-state, .empty-state {
  text-align: center;
  padding: 3rem;
  color: #7f8c8d;
}

.loading-state i {
  font-size: 2rem;
  margin-bottom: 1rem;
}

.empty-state i {
  font-size: 3rem;
  margin-bottom: 1rem;
  color: #bdc3c7;
}

.empty-state p {
  max-width: 400px;
  margin: 0 auto;
  color: #7f8c8d;
  line-height: 1.6;
}

/* RESPONSIVE */
@media (max-width: 768px) {
  .hero-title {
    font-size: 2.2rem;
  }
  
  .filters-bar {
    flex-direction: column;
    gap: 1rem;
  }
  
  .recipes-grid {
    grid-template-columns: 1fr;
  }
}
</style>