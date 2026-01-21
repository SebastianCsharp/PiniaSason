<template>
  <div class="page-container">
    <header class="page-hero">
      <div class="hero-overlay">
        <div class="hero-content">
          <h1 class="hero-title">Todas las Recetas</h1>
          <p class="hero-subtitle">{{ totalRecetas }} recetas disponibles</p>
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
          <div class="search-box">
            <i class="pi pi-search"></i>
            <input 
              v-model="searchTerm" 
              placeholder="Buscar recetas..." 
              class="search-input"
            />
          </div>
        </div>

        <div class="recipes-grid">
          <div 
            v-for="receta in recetasFiltradas" 
            :key="receta.id" 
            class="recipe-card"
            @click="verReceta(receta.id)"
          >
           <div class="recipe-image">
           <div class="recipe-badge" v-if="receta.calificacionPromedio >= 4">
            <i class="pi pi-star"></i>
            Popular
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

              <div class="recipe-rating" v-if="receta.calificacionPromedio">
                <div class="stars">
                  <i 
                    v-for="n in 5" 
                    :key="n"
                    class="pi"
                    :class="n <= Math.round(receta.calificacionPromedio) ? 'pi-star-fill' : 'pi-star'"
                  ></i>
                </div>
                <span class="rating-text">{{ receta.calificacionPromedio }}</span>
              </div>
            </div>
          </div>
        </div>
        <div v-if="cargando" class="loading-state">
          <i class="pi pi-spin pi-spinner"></i>
          Cargando recetas...
        </div>
        
        <div v-if="!cargando && recetasFiltradas.length === 0" class="empty-state">
          <i class="pi pi-inbox"></i>
          <h3>No se encontraron recetas</h3>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import { recetaService } from '../services/recetaService'

export default {
  name: 'RecetasView',
  data() {
    return {
      recetas: [],
      cargando: true,
      searchTerm: '',
      showFilters: false
    }
  },
  computed: {
    totalRecetas() {
      return this.recetas.length
    },
    recetasFiltradas() {
      if (!this.searchTerm) return this.recetas
      
      return this.recetas.filter(receta => 
        receta.titulo.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        receta.descripcion.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        receta.categoriaNombre.toLowerCase().includes(this.searchTerm.toLowerCase())
      )
    }
  },
  async mounted() {
    await this.cargarRecetas()
  },
  methods: {
    async cargarRecetas() {
      try {
        this.cargando = true
        this.recetas = await recetaService.obtenerTodas()
      } catch (error) {
        console.error('Error cargando recetas:', error)
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

.search-box {
  position: relative;
  flex: 1;
  max-width: 500px;
  margin: 0 auto;
}

.search-box i {
  position: absolute;
  left: 1rem;
  top: 50%;
  transform: translateY(-50%);
  color: #7f8c8d;
}

.search-input {
  width: 100%;
  padding: 12px 12px 12px 3rem;
  border: 1px solid #ddd;
  border-radius: 25px;
  font-size: 1rem;
}

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

.recipe-badge {
  position: absolute;
  top: 1rem;
  right: 1rem;
  background: #f39c12;
  color: white;
  padding: 0.4rem 0.8rem;
  border-radius: 15px;
  font-size: 0.8rem;
  display: flex;
  align-items: center;
  gap: 5px;
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

.recipe-rating {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.stars {
  color: #f39c12;
}

.rating-text {
  color: #7f8c8d;
  font-size: 0.9rem;
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

@media (max-width: 768px) {
  .hero-title {
    font-size: 2.2rem;
  }
  
  .filters-bar {
    flex-direction: column;
    gap: 1rem;
  }
  
  .search-box {
    width: 100%;
    max-width: 100%;
  }
  
  .recipes-grid {
    grid-template-columns: 1fr;
  }
}
</style>