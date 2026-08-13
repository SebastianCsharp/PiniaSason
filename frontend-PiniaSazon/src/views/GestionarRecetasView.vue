<template>
  <div class="page-container">
    <header class="page-hero">
      <div class="hero-overlay">
        <div class="hero-content">
          <h1 class="hero-title">Gestionar Recetas</h1>
          <p class="hero-subtitle">Administra todas las recetas del sistema</p>
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

        <div class="management-table">
          <div v-if="cargando" class="loading-state">
            <i class="pi pi-spin pi-spinner"></i>
            Cargando recetas...
          </div>

          <div v-else-if="recetasFiltradas.length === 0" class="empty-state">
            <i class="pi pi-inbox"></i>
            <h3>No se encontraron recetas</h3>
          </div>

          <div v-else class="table-responsive">
            <table class="management-table">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Título</th>
                  <th>Categoría</th>
                  <th>Tiempo</th>
                  <th>Acciones</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="receta in recetasFiltradas" :key="receta.id">
                  <td class="text-center">{{ receta.id }}</td>
                  <td>{{ receta.titulo }}</td>
                  <td>
                    <span class="badge-categoria">
                      {{ receta.categoriaNombre }}
                    </span>
                  </td>
                  <td>{{ receta.tiempoPreparacion }} min</td>
                  <td class="actions-cell">
                    <button 
                      @click="editarReceta(receta.id)" 
                      class="btn-editar"
                      title="Editar"
                    >
                      <i class="pi pi-pencil"></i>
                    </button>
                    <button 
                      @click="eliminarReceta(receta.id, receta.titulo)" 
                      class="btn-eliminar"
                      title="Eliminar"
                    >
                      <i class="pi pi-trash"></i>
                    </button>
                    <button 
                      @click="verReceta(receta.id)" 
                      class="btn-ver"
                      title="Ver detalles"
                    >
                      <i class="pi pi-eye"></i>
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
        <div v-if="mostrarModalEliminar" class="modal-overlay">
          <div class="modal-content">
            <div class="modal-header">
              <h3>Confirmar eliminación</h3>
              <button @click="mostrarModalEliminar = false" class="btn-cerrar">
                <i class="pi pi-times"></i>
              </button>
            </div>
            <div class="modal-body">
              <p>¿Estás seguro de eliminar la receta <strong>"{{ recetaAEliminarTitulo }}"</strong>?</p>
              <p class="text-muted">Esta acción no se puede deshacer.</p>
            </div>
            <div class="modal-footer">
              <button @click="mostrarModalEliminar = false" class="btn-cancelar">
                Cancelar
              </button>
              <button @click="confirmarEliminar" class="btn-eliminar-confirmar">
                <i class="pi pi-trash"></i>
                Eliminar
              </button>
            </div>
          </div>
        </div>
        <div v-if="mensajeExito" class="mensaje-exito">
          <i class="pi pi-check-circle"></i>
          {{ mensajeExito }}
        </div>
        
        <div v-if="mensajeError" class="mensaje-error">
          <i class="pi pi-exclamation-triangle"></i>
          {{ mensajeError }}
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import { recetaService } from '../services/recetaService'

export default {
  name: 'GestionarRecetasView',
  data() {
    return {
      recetas: [],
      cargando: true,
      searchTerm: '',
      mostrarModalEliminar: false,
      recetaAEliminarId: null,
      recetaAEliminarTitulo: '',
      mensajeExito: '',
      mensajeError: ''
    }
  },
  computed: {
    recetasFiltradas() {
      if (!this.searchTerm) return this.recetas
      
      const term = this.searchTerm.toLowerCase()
      return this.recetas.filter(receta => 
        receta.titulo.toLowerCase().includes(term) ||
        receta.categoriaNombre.toLowerCase().includes(term) ||
        receta.descripcion.toLowerCase().includes(term)
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
        this.mensajeError = 'Error al cargar las recetas'
      } finally {
        this.cargando = false
      }
    },

    editarReceta(id) {
      this.$router.push(`/recetas/editar/${id}`)
    },

    verReceta(id) {
      this.$router.push(`/receta/${id}`)
    },

    eliminarReceta(id, titulo) {
      this.recetaAEliminarId = id
      this.recetaAEliminarTitulo = titulo
      this.mostrarModalEliminar = true
    },

    async confirmarEliminar() {
      try {
        await recetaService.eliminarReceta(this.recetaAEliminarId)
        this.mensajeExito = `Receta "${this.recetaAEliminarTitulo}" eliminada correctamente`
        
        // recarga lista
        await this.cargarRecetas()
        
        // cierra modal y limpiar
        this.mostrarModalEliminar = false
        this.recetaAEliminarId = null
        this.recetaAEliminarTitulo = ''
        
        // oculta mensaje después de 3 segundos
        setTimeout(() => {
          this.mensajeExito = ''
        }, 3000)
        
      } catch (error) {
        console.error('Error eliminando receta:', error)
        this.mensajeError = 'Error al eliminar la receta'
        this.mostrarModalEliminar = false
      }
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
.management-table {
  width: 100%;
  background: white;
  border-radius: 10px;
  overflow: hidden;
  box-shadow: 0 5px 15px rgba(0,0,0,0.1);
  margin-bottom: 2rem;
}

.management-table table {
  width: 100%;
  border-collapse: collapse;
}

.management-table th {
  background: #2c3e50;
  color: white;
  padding: 1rem;
  text-align: left;
  font-weight: 500;
}

.management-table td {
  padding: 1rem;
  border-bottom: 1px solid #eee;
  vertical-align: middle;
}

.management-table tr:hover {
  background: #f8f9fa;
}

.management-table tr:last-child td {
  border-bottom: none;
}

.text-center {
  text-align: center;
}
.badge-categoria {
  background: #3498db;
  color: white;
  padding: 4px 12px;
  border-radius: 20px;
  font-size: 0.85rem;
  display: inline-block;
}
.actions-cell {
  display: flex;
  gap: 8px;
  justify-content: center;
}

.btn-editar, .btn-eliminar, .btn-ver {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  border: none;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.3s;
}

.btn-editar {
  background: #3498db;
  color: white;
}

.btn-editar:hover {
  background: #2980b9;
}

.btn-eliminar {
  background: #e74c3c;
  color: white;
}

.btn-eliminar:hover {
  background: #c0392b;
}

.btn-ver {
  background: #2ecc71;
  color: white;
}

.btn-ver:hover {
  background: #27ae60;
}

/* MODAL */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0,0,0,0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.modal-content {
  background: white;
  border-radius: 10px;
  width: 90%;
  max-width: 500px;
  box-shadow: 0 10px 30px rgba(0,0,0,0.3);
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.5rem;
  border-bottom: 1px solid #eee;
}

.modal-header h3 {
  margin: 0;
  color: #2c3e50;
}

.btn-cerrar {
  background: none;
  border: none;
  font-size: 1.5rem;
  color: #7f8c8d;
  cursor: pointer;
  padding: 0;
}

.modal-body {
  padding: 1.5rem;
}

.modal-body p {
  margin: 0 0 1rem 0;
}

.text-muted {
  color: #7f8c8d;
  font-size: 0.9rem;
}

.modal-footer {
  display: flex;
  gap: 1rem;
  padding: 1.5rem;
  border-top: 1px solid #eee;
  justify-content: flex-end;
}

.btn-cancelar {
  background: #95a5a6;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 6px;
  cursor: pointer;
}

.btn-cancelar:hover {
  background: #7f8c8d;
}

.btn-eliminar-confirmar {
  background: #e74c3c;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 6px;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
}

.btn-eliminar-confirmar:hover {
  background: #c0392b;
}

.mensaje-exito {
  margin-top: 1.5rem;
  padding: 1rem;
  background: #d4edda;
  color: #155724;
  border-radius: 6px;
  border: 1px solid #c3e6cb;
  display: flex;
  align-items: center;
  gap: 10px;
}

.mensaje-error {
  margin-top: 1.5rem;
  padding: 1rem;
  background: #f8d7da;
  color: #721c24;
  border-radius: 6px;
  border: 1px solid #f5c6cb;
  display: flex;
  align-items: center;
  gap: 10px;
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
  
  .management-table {
    overflow-x: auto;
  }
  
  .management-table table {
    min-width: 600px;
  }
  
  .actions-cell {
    justify-content: flex-start;
  }
}
</style>