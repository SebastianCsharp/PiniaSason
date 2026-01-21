<template>
  <div class="home-container">
    <header class="hero-section">
      <div class="hero-overlay">
        <div class="hero-content">
          <h1 class="hero-title">Tu Sazón</h1>
          <p class="hero-subtitle">Tu colección personal de recetas</p>
        </div>
      </div>
    </header>

    <section class="actions-section">
      <div class="container">
        <h2 class="section-title">GESTIÓN DE RECETAS</h2>
        <div class="actions-grid">
          <div class="action-card" @click="navigateTo('/recetas')">
            <div class="action-icon">
              <i class="pi pi-book"></i>
            </div>
            <h3>Ver Todas las Recetas</h3>
            <p>Explora todas las recetas disponibles en el sistema</p>
            <div class="endpoint-badge"></div>
          </div>
          
          <div class="action-card" @click="navigateTo('/recetas/categorias')">
            <div class="action-icon">
              <i class="pi pi-tags"></i>
            </div>
            <h3>Buscar por Categoría</h3>
            <p>Encuentra recetas por tipo de comida o categoría</p>
            <div class="endpoint-badge"></div>
          </div>
          
          <div class="action-card" @click="navigateTo('/recetas/crear')">
            <div class="action-icon">
              <i class="pi pi-plus-circle"></i>
            </div>
            <h3>Crear Nueva Receta</h3>
            <p>Agrega una nueva receta a la colección</p>
            <div class="endpoint-badge"></div>
          </div>
          
          <div class="action-card" @click="navigateTo('/recetas/gestionar')">
            <div class="action-icon">
              <i class="pi pi-pencil"></i>
            </div>
            <h3>Gestionar Recetas</h3>
            <p>Edita o elimina recetas existentes</p>
            <div class="endpoint-badge"></div>
          </div>
     <div class="action-card" @click="navigateTo('/recetas/destacadas')">
        <div class="action-icon">
         <i class="pi pi-star-fill"></i>
       </div>
         <h3>Recetas Destacadas</h3>
         <p>Descubre las recetas mejor calificadas por la comunidad</p>
       <div class="endpoint-badge"></div>
     </div>

          <div class="action-card" @click="navigateTo('/categorias-especiales')">
            <div class="action-icon">
              <i class="pi pi-list"></i>
            </div>
            <h3>Categorias especiales</h3>
            <p>Aquí tendrás las categorías mas populares</p>
            <div class="endpoint-badge"></div>
          </div>
        </div>
      </div>
    </section>

    <section class="stats-section">
      <div class="container">
        <div class="stats-grid">
          <div class="stat-item">
            <div class="stat-number">{{ totalRecetas }}</div>
            <div class="stat-label">Recetas Totales</div>
          </div>
          <div class="stat-item">
            <div class="stat-number">{{ totalCategorias }}</div>
            <div class="stat-label">Categorías</div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import { recetaService } from '../services/recetaService'

export default {
  name: "HomeView",
  data() {
    return {
      totalRecetas: 0,
      totalCategorias: 6
    }
  },
  async mounted() {
    try {
      const recetas = await recetaService.obtenerTodas()
      this.totalRecetas = recetas.length
    } catch (error) {
      console.error('Error cargando datos:', error)
    }
  },
  methods: {
    navigateTo(route) {
      this.$router.push(route)
    }
  }
}
</script>

<style scoped>
.home-container {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 20px;
}

.hero-section {
  height: 50vh;
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
  font-size: 3.5rem;
  font-weight: 300;
  margin-bottom: 1rem;
  letter-spacing: 2px;
}

.hero-subtitle {
  font-size: 1.3rem;
  opacity: 0.9;
  font-weight: 300;
}
.actions-section {
  padding: 80px 0;
  background: #f8f9fa;
}

.section-title {
  text-align: center;
  font-size: 2.2rem;
  font-weight: 300;
  color: #2c3e50;
  margin-bottom: 3rem;
}

.actions-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 2rem;
}

.action-card {
  background: white;
  padding: 2.5rem 2rem;
  border-radius: 10px;
  text-align: center;
  cursor: pointer;
  transition: all 0.3s ease;
  border: 1px solid #e9ecef;
  position: relative;
}

.action-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 30px rgba(0,0,0,0.1);
  border-color: #e74c3c;
}

.action-icon {
  font-size: 3rem;
  color: #e74c3c;
  margin-bottom: 1.5rem;
}

.action-card h3 {
  font-size: 1.3rem;
  color: #2c3e50;
  margin-bottom: 1rem;
}

.action-card p {
  color: #7f8c8d;
  line-height: 1.6;
  margin-bottom: 1.5rem;
}

.endpoint-badge {
  background: #34495e;
  color: white;
  padding: 0.4rem 0.8rem;
  border-radius: 15px;
  font-size: 0.75rem;
  font-family: 'Courier New', monospace;
  display: inline-block;
}
.stats-section {
  padding: 60px 0;
  background: white;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 2rem;
  max-width: 500px;
  margin: 0 auto;
}

.stat-item {
  text-align: center;
  padding: 2rem 1rem;
}

.stat-number {
  font-size: 3rem;
  font-weight: 300;
  color: #e74c3c;
  margin-bottom: 0.5rem;
}

.stat-label {
  color: #7f8c8d;
  font-size: 1rem;
  text-transform: uppercase;
  letter-spacing: 1px;
}
@media (max-width: 768px) {
  .hero-title {
    font-size: 2.5rem;
  }
  
  .actions-grid {
    grid-template-columns: 1fr;
  }
  
  .stats-grid {
    grid-template-columns: 1fr;
  }
}
</style>