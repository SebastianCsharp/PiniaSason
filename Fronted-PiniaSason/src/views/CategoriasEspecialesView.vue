<template>
  <div class="page-container">
    <header class="page-hero">
      <div class="hero-overlay">
        <div class="hero-content">
          <h1 class="hero-title">Categorías Especiales</h1>
          <p class="hero-subtitle">Las categorías más populares de la comunidad</p>
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
          
          <button 
            v-if="categoriaSeleccionada" 
            @click="limpiarSeleccion" 
            class="btn-limpiar"
          >
            <i class="pi pi-times"></i>
            Volver a categorías
          </button>
        </div>

        <div v-if="!categoriaSeleccionada" class="categorias-container">
          <h2 class="section-title">Categorías más populares</h2>
          
          <div v-if="cargando" class="loading">
            <i class="pi pi-spin pi-spinner"></i>
            Cargando categorías...
          </div>

          <div v-if="error" class="error-message">
            <i class="pi pi-exclamation-triangle"></i>
            {{ error }}
          </div>

          <div v-if="!cargando && !error && categorias.length > 0" class="categorias-grid">
            <div 
              v-for="categoria in categorias" 
              :key="categoria.id" 
              class="categoria-card"
              @click="seleccionarCategoria(categoria)"
            >
              <div class="categoria-content">
                <h3 class="categoria-nombre">{{ categoria.nombre }}</h3>
                <div class="categoria-stats">
                  <span class="receta-count">
                    <i class="pi pi-book"></i>
                    {{ categoria.cantidadRecetas }} receta{{ categoria.cantidadRecetas !== 1 ? 's' : '' }}
                  </span>
                </div>
                <p class="categoria-desc">
                  {{ obtenerDescripcion(categoria.cantidadRecetas) }}
                </p>
              </div>
              <div class="categoria-footer">
                <button class="btn-ver-recetas">
                  Ver recetas
                  <i class="pi pi-arrow-right"></i>
                </button>
              </div>
            </div>
          </div>

          <div v-if="!cargando && !error && categorias.length === 0" class="empty-state">
            <i class="pi pi-inbox"></i>
            <h3>No hay categorías con recetas aún</h3>
            <p>Crea recetas para ver las categorías más populares aquí.</p>
            <button @click="$router.push('/recetas/crear')" class="btn-crear-receta">
              <i class="pi pi-plus"></i>
              Crear mi primera receta
            </button>
          </div>
        </div>

        <div v-if="categoriaSeleccionada" class="recetas-container">
          <div class="categoria-header">
            <h2 class="categoria-titulo">
              <i class="pi pi-arrow-left" @click="limpiarSeleccion" style="cursor: pointer; margin-right: 10px;"></i>
              Recetas de: {{ categoriaSeleccionada.nombre }}
              <span class="receta-count-header">
                {{ categoriaSeleccionada.cantidadRecetas }} receta{{ categoriaSeleccionada.cantidadRecetas !== 1 ? 's' : '' }}
              </span>
            </h2>
          </div>

          <div v-if="cargandoRecetas" class="loading">
            <i class="pi pi-spin pi-spinner"></i>
            Cargando recetas...
          </div>

          <div v-if="errorRecetas" class="error-message">
            <i class="pi pi-exclamation-triangle"></i>
            {{ errorRecetas }}
          </div>

          <div v-if="!cargandoRecetas && !errorRecetas" class="recipes-grid">
            <div 
              v-for="receta in recetas" 
              :key="receta.id" 
              class="recipe-card"
              @click="verRecetaDetalle(receta.id)"
            >
              <div class="recipe-image">
                <div class="recipe-badge" v-if="receta.calificacionPromedio >= 4">
                  <i class="pi pi-star"></i>
                  Popular
                </div>
              </div>
              
              <div class="recipe-content">
                <h3 class="recipe-title">{{ receta.titulo }}</h3>
                <p class="recipe-desc">{{ receta.descripcion?.substring(0, 100) || 'Sin descripción' }}...</p>
                
                <div class="recipe-meta">
                  <span class="time">
                    <i class="pi pi-clock"></i>
                    {{ receta.tiempoPreparacion }} min
                  </span>
                  <span class="category">
                    <i class="pi pi-tag"></i>
                    {{ categoriaSeleccionada.nombre }}
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
                  <span class="rating-text">{{ receta.calificacionPromedio.toFixed(1) }}</span>
                </div>
              </div>
            </div>
          </div>

          <div v-if="!cargandoRecetas && !errorRecetas && recetas.length === 0" class="empty-state">
            <i class="pi pi-inbox"></i>
            <h3>No hay recetas en esta categoría</h3>
            <p>Sé el primero en crear una receta para {{ categoriaSeleccionada.nombre }}.</p>
            <button @click="$router.push('/recetas/crear')" class="btn-crear-receta">
              <i class="pi pi-plus"></i>
              Crear receta
            </button>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import { categoriaService } from '../services/categoriaService'
import { recetaService } from '../services/recetaService'

export default {
  name: 'CategoriasEspecialesView',
  data() {
    return {
      categorias: [],
      categoriaSeleccionada: null,
      recetas: [],
      cargando: false,
      cargandoRecetas: false,
      error: '',
      errorRecetas: ''
    }
  },
  async mounted() {
    await this.cargarCategorias()
  },
  methods: {
    async cargarCategorias() {
      this.cargando = true
      this.error = ''
      
      try {
        this.categorias = await categoriaService.obtenerCategoriasMasUsadas()
      } catch (error) {
        console.error('Error cargando categorías:', error)
        this.error = 'No se pudieron cargar las categorías. Intenta nuevamente.'
      } finally {
        this.cargando = false
      }
    },
    
    async seleccionarCategoria(categoria) {
      this.categoriaSeleccionada = categoria
      this.cargarRecetasDeCategoria(categoria.id)
    },
    
   async cargarRecetasDeCategoria(categoriaId) {
      this.cargandoRecetas = true
      this.errorRecetas = ''
      this.recetas = []
      
      try {
        const categoria = this.categorias.find(c => c.id === categoriaId)
        
        if (!categoria) {
          throw new Error('Categoría no encontrada')
        }
        this.recetas = await recetaService.obtenerPorCategoria(categoria.nombre)
        
      } catch (error) {
        console.error('Error cargando recetas:', error)
        this.errorRecetas = 'No se pudieron cargar las recetas. Intenta nuevamente.'
      } finally {
        this.cargandoRecetas = false
      }
    },
    
    limpiarSeleccion() {
      this.categoriaSeleccionada = null
      this.recetas = []
      this.errorRecetas = ''
    },
    
    verRecetaDetalle(recetaId) {
      this.$router.push(`/receta/${recetaId}`)
    },
    
    obtenerDescripcion(cantidad) {
      if (cantidad === 0) return 'Aún no hay recetas en esta categoría.'
      if (cantidad === 1) return 'Una receta especial te espera en esta categoría.'
      if (cantidad < 5) return `Descubre ${cantidad} recetas únicas en esta categoría.`
      return `Explora ${cantidad} recetas populares en esta categoría.`
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

.btn-limpiar {
  background: transparent;
  border: 2px solid #95a5a6;
  color: #95a5a6;
  padding: 10px 20px;
  border-radius: 25px;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
  transition: all 0.3s ease;
}

.btn-limpiar:hover {
  background: #95a5a6;
  color: white;
}

/* Título de sección */
.section-title {
  color: #2c3e50;
  font-size: 1.8rem;
  margin-bottom: 2rem;
  font-weight: 600;
}

/* CATEGORÍAS */
.categorias-container {
  margin-top: 2rem;
}

.categorias-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 2rem;
  margin-top: 2rem;
}

.categoria-card {
  background: white;
  border-radius: 12px;
  overflow: hidden;
  box-shadow: 0 6px 16px rgba(0,0,0,0.1);
  transition: all 0.3s ease;
  cursor: pointer;
  display: flex;
  flex-direction: column;
}

.categoria-card:hover {
  transform: translateY(-8px);
  box-shadow: 0 12px 24px rgba(0,0,0,0.15);
}

.categoria-content {
  padding: 1.8rem;
  flex-grow: 1;
}

.categoria-nombre {
  font-size: 1.5rem;
  color: #2c3e50;
  margin-bottom: 1rem;
  font-weight: 600;
  line-height: 1.2;
}

.categoria-stats {
  display: flex;
  align-items: center;
  margin-bottom: 1rem;
}

.receta-count {
  background: linear-gradient(135deg, #e8f4fc, #d4e8f7);
  color: #2980b9;
  padding: 0.5rem 1rem;
  border-radius: 25px;
  font-size: 1rem;
  font-weight: 500;
  display: inline-flex;
  align-items: center;
  gap: 8px;
  box-shadow: 0 2px 4px rgba(41, 128, 185, 0.2);
}

.categoria-desc {
  color: #5d6d7e;
  font-size: 0.95rem;
  line-height: 1.5;
  margin-top: 0.5rem;
}

.categoria-footer {
  background: #f8fafc;
  padding: 1.2rem 1.8rem;
  border-top: 1px solid #eaeaea;
}

.btn-ver-recetas {
  background: linear-gradient(135deg, #e74c3c, #c0392b);
  color: white;
  border: none;
  padding: 0.8rem 1.5rem;
  border-radius: 8px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
  font-size: 1rem;
  font-weight: 500;
  width: 100%;
  transition: all 0.3s ease;
}

.btn-ver-recetas:hover {
  background: linear-gradient(135deg, #c0392b, #a93226);
  transform: scale(1.02);
}


.recetas-container {
  margin-top: 2rem;
}

.categoria-header {
  margin-bottom: 2rem;
  padding-bottom: 1rem;
  border-bottom: 2px solid #eee;
}

.categoria-titulo {
  color: #2c3e50;
  font-size: 1.8rem;
  display: flex;
  align-items: center;
  font-weight: 600;
}

.receta-count-header {
  background: #3498db;
  color: white;
  padding: 0.3rem 1rem;
  border-radius: 20px;
  font-size: 1rem;
  margin-left: 15px;
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

.loading {
  text-align: center;
  padding: 4rem 2rem;
  color: #3498db;
  font-size: 1.2rem;
}

.loading i {
  font-size: 2rem;
  margin-right: 15px;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.error-message {
  background: linear-gradient(135deg, #fdeaea, #f8d7da);
  color: #721c24;
  padding: 1.5rem;
  border-radius: 10px;
  border: 2px solid #f5c6cb;
  margin-top: 2rem;
  text-align: center;
  font-size: 1.1rem;
}

.error-message i {
  font-size: 1.5rem;
  margin-right: 10px;
  color: #e74c3c;
}

.empty-state {
  text-align: center;
  padding: 4rem 2rem;
  background: #f8f9fa;
  border-radius: 12px;
  border: 2px dashed #bdc3c7;
  margin-top: 2rem;
}

.empty-state i {
  font-size: 4rem;
  color: #bdc3c7;
  margin-bottom: 1.5rem;
}

.empty-state h3 {
  color: #2c3e50;
  margin-bottom: 1rem;
  font-size: 1.8rem;
}

.empty-state p {
  color: #7f8c8d;
  margin-bottom: 2rem;
  font-size: 1.1rem;
}

.btn-crear-receta {
  background: linear-gradient(135deg, #2ecc71, #27ae60);
  color: white;
  border: none;
  padding: 1rem 2rem;
  border-radius: 8px;
  font-size: 1.1rem;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  gap: 10px;
  transition: all 0.3s ease;
}

.btn-crear-receta:hover {
  background: linear-gradient(135deg, #27ae60, #219653);
  transform: scale(1.05);
}

@media (max-width: 768px) {
  .hero-title {
    font-size: 2.2rem;
  }
  
  .filters-bar {
    flex-direction: column;
    gap: 1rem;
  }
  
  .categorias-grid,
  .recipes-grid {
    grid-template-columns: 1fr;
    gap: 1.5rem;
  }
  
  .categoria-content {
    padding: 1.5rem;
  }
  
  .categoria-nombre {
    font-size: 1.3rem;
  }
  
  .recipe-image {
    height: 120px;
  }
}
</style>