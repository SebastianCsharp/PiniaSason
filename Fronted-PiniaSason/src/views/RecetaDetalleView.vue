<template>
  <div class="receta-detalle-view">
    <!-- heat pa navegar -->
    <div class="navigation-header">
      <div class="container">
        <button @click="$router.back()" class="btn-volver">
          <i class="pi pi-arrow-left"></i>
          Volver a Recetas
        </button>
        <div class="breadcrumb">
          <span @click="$router.push('/')" class="breadcrumb-item">Inicio</span>
          <i class="pi pi-chevron-right"></i>
          <span @click="$router.push('/recetas')" class="breadcrumb-item">Recetas</span>
          <i class="pi pi-chevron-right"></i>
          <span class="breadcrumb-item active">{{ receta?.titulo }}</span>
        </div>
      </div>
    </div>

    <div class="container main-container">
      <div v-if="cargando" class="loading-state">
        <i class="pi pi-spin pi-spinner"></i>
        <p>Cargando receta...</p>
      </div>
      <div v-if="error && !cargando" class="error-state">
        <i class="pi pi-exclamation-triangle"></i>
        <p>{{ error }}</p>
        <button @click="cargarReceta" class="btn-reintentar">Reintentar</button>
      </div>

      <div v-if="receta && !cargando" class="receta-content">
        <div class="receta-hero">
          <div class="hero-background">
            <div class="background-overlay"></div>
          </div>
          <div class="hero-content">
            <div class="receta-header">
              <div class="category-badge" v-if="recetaCategoria">
                {{ recetaCategoria }}
              </div>
              <h1 class="receta-titulo">{{ receta.titulo }}</h1>
              <div class="receta-meta">
                <span class="meta-item">
                  <i class="pi pi-clock"></i>
                  {{ receta.tiempoPreparacion }} min
                </span>
                <span class="meta-item" v-if="recetaCalificacion > 0">
                  <i class="pi pi-star"></i>
                  {{ recetaCalificacion.toFixed(1) }}
                </span>
                <span class="meta-item">
                  <i class="pi pi-comments"></i>
                  {{ comentarios.length }}
                </span>
              </div>
            </div>
          </div>
        </div>
        <div class="receta-layout">
          <div class="left-column">
            <div class="info-card">
              <div class="card-header">
                <i class="pi pi-file"></i>
                <h3>Descripción</h3>
              </div>
              <div class="card-content">
                <p class="descripcion-texto">{{ receta.descripcion }}</p>
              </div>
            </div>
            <div class="info-card" v-if="recetaIngredientes.length > 0">
              <div class="card-header">
                <i class="pi pi-shopping-bag"></i>
                <h3>Ingredientes</h3>
                <span class="badge">{{ recetaIngredientes.length }} items</span>
              </div>
              <div class="card-content">
                <div class="ingredientes-list">
                  <div 
                    v-for="(ingrediente, index) in recetaIngredientes" 
                    :key="index" 
                    class="ingrediente-item"
                  >
                    <div class="ingrediente-info">
                      <span class="ingrediente-nombre">{{ ingrediente.nombre }}</span>
                      <span class="ingrediente-cantidad">
                        {{ ingrediente.cantidad }} {{ ingrediente.unidadMedida }}
                      </span>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          
          <div class="right-column">
            <div class="info-card" v-if="pasosFormateados.length > 0">
              <div class="card-header">
                <i class="pi pi-play"></i>
                <h3>Preparación</h3>
                <span class="badge">{{ pasosFormateados.length }} pasos</span>
              </div>
              <div class="card-content">
                <div class="pasos-list">
                  <div 
                    v-for="(paso, index) in pasosFormateados" 
                    :key="index" 
                    class="paso-item"
                  >
                    <div class="paso-header">
                      <div class="paso-numero">Paso {{ index + 1 }}</div>
                      <div class="paso-indicator"></div>
                    </div>
                    <div class="paso-texto">{{ paso }}</div>
                  </div>
                </div>
              </div>
            </div>

            <div class="info-card">
              <div class="card-header">
                <i class="pi pi-comments"></i>
                <h3>Comentarios</h3>
                <span class="badge">{{ comentarios.length }}</span>
              </div>
              <div class="card-content">
                <div class="comentario-form-section">
                  <h4>Agregar Comentario</h4>
                  <form @submit.prevent="agregarComentario" class="comentario-form">
                    <div class="form-row">
                      <div class="form-group">
                        <input 
                          v-model="nuevoComentario.nombreUsuario" 
                          placeholder="Tu nombre"
                          class="input-field"
                          required
                        >
                      </div>
                      <div class="form-group">
                        <div class="calificacion-input">
                          <label>Calificación:</label>
                          <div class="stars">
                            <span 
                              v-for="star in 5" 
                              :key="star" 
                              class="star"
                              :class="{ active: nuevoComentario.calificacion >= star }"
                              @click="nuevoComentario.calificacion = star"
                            >
                              <i class="pi pi-star"></i>
                            </span>
                          </div>
                        </div>
                      </div>
                    </div>
                    <div class="form-group">
                      <textarea 
                        v-model="nuevoComentario.texto" 
                        placeholder="Escribe tu comentario..."
                        rows="3"
                        class="textarea-field"
                        required
                      ></textarea>
                    </div>
                    <button 
                      type="submit" 
                      class="btn-primary"
                      :disabled="!nuevoComentario.texto || nuevoComentario.calificacion === 0 || enviandoComentario"
                    >
                      <i class="pi pi-send"></i>
                      {{ enviandoComentario ? 'Enviando...' : 'Publicar Comentario' }}
                    </button>
                  </form>
                </div>
                <div v-if="comentariosCargando" class="loading-comentarios">
                  <i class="pi pi-spin pi-spinner"></i>
                  <p>Cargando comentarios...</p>
                </div>

                <div v-else-if="comentarios.length > 0" class="comentarios-list">
                  <div 
                    v-for="comentario in comentarios" 
                    :key="comentario.id" 
                    class="comentario-item"
                  >
                    <div class="comentario-header">
                      <div class="user-info">
                        <div class="avatar">
                          <i class="pi pi-user"></i>
                        </div>
                        <div class="user-details">
                          <span class="username">{{ comentario.nombreUsuario }}</span>
                          <div class="rating">
                            <span 
                              v-for="star in 5" 
                              :key="star" 
                              class="star small"
                              :class="{ active: comentario.calificacion >= star }"
                            >
                              <i class="pi pi-star"></i>
                            </span>
                          </div>
                        </div>
                      </div>
                      <div style="display: flex; flex-direction: column; align-items: flex-end; gap: 8px;">
                        <span class="fecha">{{ formatearFecha(comentario.fechaCreacion) }}</span>
                        <div style="display: flex; gap: 8px;">
                          <button 
                            @click="iniciarEdicion(comentario)"
                            style="background: transparent; border: 1px solid #3498db; color: #3498db; padding: 6px; border-radius: 4px; cursor: pointer; width: 32px; height: 32px; display: flex; align-items: center; justify-content: center;"
                            title="Editar comentario"
                          >
                            <i class="pi pi-pencil"></i>
                          </button>
                          <button 
                            @click="confirmarEliminar(comentario)"
                            style="background: transparent; border: 1px solid #e74c3c; color: #e74c3c; padding: 6px; border-radius: 4px; cursor: pointer; width: 32px; height: 32px; display: flex; align-items: center; justify-content: center;"
                            title="Eliminar comentario"
                          >
                            <i class="pi pi-trash"></i>
                          </button>
                        </div>
                      </div>
                    </div>

                    <div v-if="!comentario.editando" class="comentario-content">
                      {{ comentario.texto }}
                    </div>

                    <div v-else style="margin-top: 12px; padding: 16px; background: white; border-radius: 8px; border: 1px solid #e2e8f0;">
                      <textarea 
                        v-model="comentario.textoEditado"
                        rows="3"
                        class="textarea-field"
                        placeholder="Edita tu comentario..."
                      ></textarea>
                      <div style="display: flex; align-items: center; gap: 12px; margin: 12px 0;">
                        <label style="color: #374151; font-size: 0.875rem; font-weight: 500;">Calificación:</label>
                        <div class="stars">
                          <span 
                            v-for="n in 5" 
                            :key="n"
                            class="star"
                            :class="{ active: comentario.calificacionEditada >= n }"
                            @click="comentario.calificacionEditada = n"
                          >
                            <i class="pi pi-star"></i>
                          </span>
                        </div>
                      </div>
                      <div style="display: flex; gap: 12px; margin-top: 16px;">
                        <button 
                          @click="guardarEdicion(comentario)"
                          :disabled="guardandoEdicion"
                          style="background: #e74c3c; color: white; border: none; padding: 8px 16px; border-radius: 6px; cursor: pointer; font-weight: 600; display: flex; align-items: center; gap: 8px; font-size: 0.875rem;"
                        >
                          <i class="pi pi-check"></i>
                          {{ guardandoEdicion ? 'Guardando...' : 'Guardar' }}
                        </button>
                        <button 
                          @click="cancelarEdicion(comentario)"
                          style="background: #6b7280; color: white; border: none; padding: 8px 16px; border-radius: 6px; cursor: pointer; font-weight: 600; display: flex; align-items: center; gap: 8px; font-size: 0.875rem;"
                        >
                          <i class="pi pi-times"></i>
                          Cancelar
                        </button>
                      </div>
                    </div>
                  </div>
                </div>

                <div v-else class="empty-state">
                  <i class="pi pi-inbox"></i>
                  <p>No hay comentarios aún</p>
                  <span>Sé el primero en comentar esta receta</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { recetaService } from '../services/recetaService'
import { comentarioService } from '../services/comentarioService'

export default {
  name: 'RecetaDetalleView',
  data() {
    return {
      receta: null,
      cargando: true,
      error: null,
      comentarios: [],
      comentariosCargando: false,
      enviandoComentario: false,
      guardandoEdicion: false,
      nuevoComentario: {
        texto: '',
        calificacion: 0,
        nombreUsuario: ''
      }
    }
  },
  computed: {
    recetaCategoria() {
      return this.receta?.categoriaNombre || ''
    },
    recetaCalificacion() {
      return this.receta?.calificacionPromedio || 0
    },
    recetaIngredientes() {
      return this.receta?.ingredientes || []
    },
    pasosFormateados() {
      const pasos = this.receta?.pasos || ''
      if (!pasos) return []
      return pasos.split('\n').filter(paso => paso.trim())
    }
  },
  async mounted() {
    await this.cargarReceta()
    await this.cargarComentarios()
  },
  methods: {
    async cargarReceta() {
      try {
        this.cargando = true
        this.error = null
        const recetaId = this.$route.params.id
        this.receta = await recetaService.obtenerPorId(recetaId)
      } catch (err) {
        console.error('❌ Error cargando receta:', err)
        this.error = 'No se pudo cargar la receta. Por favor, intenta nuevamente.'
      } finally {
        this.cargando = false
      }
    },
    async cargarComentarios() {
      try {
        this.comentariosCargando = true
        const recetaId = this.$route.params.id
        const comentariosData = await comentarioService.obtenerPorReceta(recetaId)
        
        this.comentarios = comentariosData.map(comentario => ({
          ...comentario,
          editando: false,
          textoEditado: comentario.texto,
          calificacionEditada: comentario.calificacion
        }))
      } catch (err) {
        console.error(err)
        this.comentarios = []
      } finally {
        this.comentariosCargando = false
      }
    },

    async agregarComentario() {
      try {
        this.enviandoComentario = true
        
        const comentarioData = {
          texto: this.nuevoComentario.texto,
          calificacion: this.nuevoComentario.calificacion,
          nombreUsuario: this.nuevoComentario.nombreUsuario.trim()
        }
        await comentarioService.crear(this.$route.params.id, comentarioData)
        this.nuevoComentario = { 
          texto: '', 
          calificacion: 0, 
          nombreUsuario: '' 
        }
        await this.cargarComentarios()
        
      } catch (error) {
        console.error(error)
      } finally {
        this.enviandoComentario = false
      }
    },

    iniciarEdicion(comentario) {
      this.comentarios.forEach(c => c.editando = false)
      comentario.editando = true
      comentario.textoEditado = comentario.texto
      comentario.calificacionEditada = comentario.calificacion
    },

    cancelarEdicion(comentario) {
      comentario.editando = false
      comentario.textoEditado = comentario.texto
      comentario.calificacionEditada = comentario.calificacion
    },
    async guardarEdicion(comentario) {
      if (!comentario.textoEditado.trim()) {
        alert('El comentario no puede estar vacío')
        return
      }

      this.guardandoEdicion = true

      try {
        const datosActualizados = {
          texto: comentario.textoEditado,
          calificacion: comentario.calificacionEditada
        }

        await comentarioService.actualizar(this.$route.params.id, comentario.id, datosActualizados)

        comentario.texto = comentario.textoEditado
        comentario.calificacion = comentario.calificacionEditada
        comentario.editando = false

        await this.cargarReceta()

      } catch (err) {
        console.error(err)
        alert('Error al actualizar el comentario')
      } finally {
        this.guardandoEdicion = false
      }
    },
    async confirmarEliminar(comentario) {
      if (confirm('¿Estás seguro de que quieres eliminar este comentario?')) {
        try {
          await comentarioService.eliminar(this.$route.params.id, comentario.id)
          
          const index = this.comentarios.findIndex(c => c.id === comentario.id)
          if (index !== -1) {
            this.comentarios.splice(index, 1)
          }
          
          await this.cargarReceta()

        } catch (error) {
          console.error(error)
          alert('Error al eliminar el comentario')
        }
      }
    },

    formatearFecha(fecha) {
      if (!fecha) return 'Fecha no disponible'
      return new Date(fecha).toLocaleDateString('es-ES', {
        year: 'numeric',
        month: 'short',
        day: 'numeric'
      })
    }
  }
}
</script>

<style scoped>
.receta-detalle-view {
  min-height: 100vh;
  background: #f8fafc;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 20px;
}
.navigation-header {
  background: white;
  border-bottom: 1px solid #e2e8f0;
  padding: 16px 0;
  position: sticky;
  top: 0;
  z-index: 100;
}

.navigation-header .container {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 20px;
}
.btn-volver {
  background: transparent;
  border: 2px solid #e74c3c;
  color: #e74c3c;
  padding: 10px 20px;
  border-radius: 25px;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  gap: 8px;
  margin: 20px 0;
  transition: all 0.3s ease;
  font-weight: 500;
}

.btn-volver:hover {
  background: #e74c3c;
  color: white;
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(231, 76, 60, 0.3);
}

.breadcrumb {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 0.875rem;
  color: #6b7280;
}

.breadcrumb-item {
  cursor: pointer;
  transition: color 0.2s ease;
}

.breadcrumb-item:hover {
  color: #374151;
}

.breadcrumb-item.active {
  color: #e74c3c;
  font-weight: 500;
}
.receta-hero {
  position: relative;
  min-height: 400px;
  display: flex;
  align-items: center;
  border-radius: 12px;
  overflow: hidden;
  margin: 24px 0;
}

.hero-background {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-image: url('https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2070&q=80');
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
}

.background-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5); 
}

.hero-content {
  position: relative;
  z-index: 2;
  width: 100%;
  padding: 60px 40px;
}

.receta-header {
  color: white;
  text-align: center;
}

.category-badge {
  display: inline-block;
  background: rgba(231, 76, 60, 0.9);
  color: white;
  padding: 8px 20px;
  border-radius: 20px;
  font-size: 0.875rem;
  font-weight: 600;
  margin-bottom: 20px;
  backdrop-filter: blur(10px);
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.receta-titulo {
  font-size: 3.5rem;
  font-weight: 700;
  margin-bottom: 24px;
  line-height: 1.1;
  text-shadow: 2px 2px 8px rgba(0,0,0,0.7);
}

.receta-meta {
  display: flex;
  justify-content: center;
  gap: 30px;
  flex-wrap: wrap;
}

.meta-item {
  display: flex;
  align-items: center;
  gap: 10px;
  font-size: 1.1rem;
  background: rgba(255, 255, 255, 0.15);
  padding: 10px 20px;
  border-radius: 25px;
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.3);
  transition: all 0.3s ease;
}

.meta-item:hover {
  background: rgba(255, 255, 255, 0.25);
  transform: translateY(-2px);
}
.receta-layout {
  display: grid;
  grid-template-columns: 1fr 1.5fr;
  gap: 24px;
  margin-bottom: 40px;
}

.left-column,
.right-column {
  display: flex;
  flex-direction: column;
  gap: 24px;
}
.info-card {
  background: white;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
  border: 1px solid #e2e8f0;
  overflow: hidden;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.info-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 16px rgba(0,0,0,0.12);
}

.card-header {
  background: #f8fafc;
  padding: 20px 24px;
  border-bottom: 1px solid #e2e8f0;
  display: flex;
  align-items: center;
  gap: 12px;
}

.card-header i {
  color: #e74c3c;
  font-size: 1.25rem;
}

.card-header h3 {
  color: #1a202c;
  font-size: 1.25rem;
  font-weight: 600;
  margin: 0;
  flex: 1;
}

.badge {
  background: #e2e8f0;
  color: #4a5568;
  padding: 4px 8px;
  border-radius: 6px;
  font-size: 0.75rem;
  font-weight: 600;
}

.card-content {
  padding: 24px;
}
.descripcion-texto {
  color: #4a5568;
  line-height: 1.6;
  font-size: 1rem;
  margin: 0;
}
.ingredientes-list {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.ingrediente-item {
  display: flex;
  align-items: center;
  padding: 12px 0;
  border-bottom: 1px solid #f1f5f9;
  transition: background-color 0.2s ease;
}

.ingrediente-item:hover {
  background-color: #f8fafc;
  border-radius: 6px;
  padding: 12px;
  margin: 0 -12px;
}

.ingrediente-item:last-child {
  border-bottom: none;
}

.ingrediente-info {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 100%;
}

.ingrediente-nombre {
  color: #1a202c;
  font-weight: 500;
}

.ingrediente-cantidad {
  color: #e74c3c;
  font-weight: 600;
  background: #fff5f5;
  padding: 6px 12px;
  border-radius: 6px;
  font-size: 0.875rem;
  border: 1px solid #fed7d7;
}
.pasos-list {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.paso-item {
  display: flex;
  gap: 16px;
  align-items: flex-start;
  padding: 16px;
  background: #f8fafc;
  border-radius: 8px;
  border-left: 4px solid #e74c3c;
  transition: all 0.2s ease;
}

.paso-item:hover {
  background: #f1f5f9;
  transform: translateX(4px);
}

.paso-header {
  display: flex;
  align-items: center;
  gap: 12px;
  min-width: 120px;
}

.paso-numero {
  color: #e74c3c;
  font-weight: 600;
  font-size: 0.875rem;
}

.paso-indicator {
  width: 8px;
  height: 8px;
  background: #e74c3c;
  border-radius: 50%;
  flex-shrink: 0;
}

.paso-texto {
  color: #4a5568;
  line-height: 1.6;
  flex: 1;
  padding-top: 2px;
}
.comentario-form-section {
  margin-bottom: 24px;
  padding-bottom: 24px;
  border-bottom: 1px solid #e2e8f0;
}

.comentario-form-section h4 {
  color: #1a202c;
  margin-bottom: 16px;
  font-size: 1.125rem;
  font-weight: 600;
}

.comentario-form {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.input-field,
.textarea-field {
  padding: 12px;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  font-size: 0.875rem;
  transition: border-color 0.2s ease;
  background: white;
}

.input-field:focus,
.textarea-field:focus {
  outline: none;
  border-color: #e74c3c;
  box-shadow: 0 0 0 3px rgba(231, 76, 60, 0.1);
}

.textarea-field {
  resize: vertical;
  min-height: 80px;
}

.calificacion-input {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.calificacion-input label {
  color: #374151;
  font-size: 0.875rem;
  font-weight: 500;
}

.stars {
  display: flex;
  gap: 4px;
}

.star {
  cursor: pointer;
  color: #d1d5db;
  transition: all 0.2s ease;
  font-size: 1.25rem;
}

.star.small {
  font-size: 0.875rem;
}

.star.active {
  color: #f59e0b;
}

.star:hover {
  color: #f59e0b;
  transform: scale(1.1);
}

.btn-primary {
  background: #e74c3c;
  color: white;
  border: none;
  padding: 12px 24px;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 8px;
  transition: all 0.2s ease;
  align-self: flex-start;
}

.btn-primary:hover:not(:disabled) {
  background: #c0392b;
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(231, 76, 60, 0.3);
}

.btn-primary:disabled {
  background: #9ca3af;
  cursor: not-allowed;
  transform: none;
  box-shadow: none;
}
.comentarios-list {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.comentario-item {
  background: #f8fafc;
  padding: 20px;
  border-radius: 8px;
  border: 1px solid #e2e8f0;
  transition: all 0.2s ease;
}

.comentario-item:hover {
  background: white;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
}

.comentario-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 12px;
}

.user-info {
  display: flex;
  align-items: center;
  gap: 12px;
}

.avatar {
  width: 40px;
  height: 40px;
  background: #e74c3c;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
}

.user-details {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.username {
  color: #1a202c;
  font-weight: 600;
  font-size: 0.875rem;
}

.rating {
  display: flex;
  gap: 2px;
}

.fecha {
  color: #6b7280;
  font-size: 0.75rem;
}

.comentario-content {
  color: #4a5568;
  line-height: 1.5;
  font-size: 0.875rem;
}
.empty-state {
  text-align: center;
  padding: 40px 20px;
  color: #6b7280;
}

.empty-state i {
  font-size: 3rem;
  margin-bottom: 12px;
  color: #9ca3af;
}

.empty-state p {
  font-size: 1rem;
  margin-bottom: 8px;
  font-weight: 500;
}

.empty-state span {
  font-size: 0.875rem;
}

.loading-state,
.error-state {
  text-align: center;
  padding: 80px 20px;
  background: white;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
}

.loading-state i,
.error-state i {
  font-size: 3rem;
  margin-bottom: 16px;
  color: #6b7280;
}

.error-state {
  color: #e74c3c;
}

.btn-reintentar {
  background: #e74c3c;
  color: white;
  border: none;
  padding: 12px 24px;
  border-radius: 6px;
  cursor: pointer;
  margin-top: 16px;
  font-weight: 500;
  transition: background 0.2s ease;
}

.btn-reintentar:hover {
  background: #c0392b;
}

.loading-comentarios {
  text-align: center;
  padding: 40px 20px;
  color: #6b7280;
}

.loading-comentarios i {
  font-size: 2rem;
  margin-bottom: 12px;
}
@media (max-width: 968px) {
  .receta-layout {
    grid-template-columns: 1fr;
    gap: 20px;
  }
  
  .receta-titulo {
    font-size: 2.5rem;
  }
  
  .form-row {
    grid-template-columns: 1fr;
  }
}

@media (max-width: 768px) {
  .container {
    padding: 0 16px;
  }
  
  .navigation-header .container {
    flex-direction: column;
    align-items: flex-start;
    gap: 12px;
  }
  
  .receta-hero {
    min-height: 300px;
    margin: 16px 0;
  }
  
  .hero-content {
    padding: 40px 20px;
  }
  
  .receta-titulo {
    font-size: 2rem;
  }
  
  .receta-meta {
    gap: 12px;
  }
  
  .meta-item {
    font-size: 0.875rem;
    padding: 8px 16px;
  }
  
  .card-content {
    padding: 20px;
  }
  
  .paso-item {
    flex-direction: column;
    gap: 8px;
  }
  
  .paso-header {
    min-width: auto;
  }
}

@media (max-width: 480px) {
  .receta-titulo {
    font-size: 1.75rem;
  }
  
  .card-header {
    padding: 16px 20px;
  }
  
  .card-header h3 {
    font-size: 1.125rem;
  }
  
  .btn-volver {
    padding: 8px 16px;
    font-size: 0.875rem;
  }
}
</style>