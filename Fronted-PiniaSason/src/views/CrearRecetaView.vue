<template>
  <div class="page-container">
    <header class="page-hero">
      <div class="hero-overlay">
        <div class="hero-content">
          <h1 class="hero-title">Crear Nueva Receta</h1>
          <p class="hero-subtitle">Comparte tu receta con la comunidad</p>
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
        </div>

        <div class="form-card">
          <form @submit.prevent="crearReceta" novalidate>
            <div class="form-section">
              <h3 class="section-title">
                Información de tu receta
              </h3>
              
              <div class="form-row">
                <div class="form-group">
                  <label for="titulo" class="form-label">Título</label>
                  <input 
                    id="titulo" 
                    v-model="recetaData.titulo" 
                    type="text" 
                    required
                    placeholder="Ej: Arroz con pollo"
                    class="form-input"
                  >
                </div>
                
                <div class="form-group">
                  <label for="tiempoPreparacion" class="form-label">Tiempo (min)</label>
                  <input 
                    id="tiempoPreparacion" 
                    v-model="recetaData.tiempoPreparacion" 
                    type="number" 
                    min="1"
                    required
                    class="form-input"
                  >
                </div>
              </div>
              
              <div class="form-group">
                <label for="descripcion" class="form-label">Descripción</label>
                <textarea 
                  id="descripcion" 
                  v-model="recetaData.descripcion" 
                  rows="3"
                  placeholder="Describe tu receta..."
                  class="form-textarea"
                  required
                ></textarea>
              </div>
              
              <div class="form-group">
                <label for="categoria" class="form-label">Categoría</label>
                <select 
                  id="categoria" 
                  v-model="recetaData.categoriaId" 
                  required
                  class="form-select"
                >
                  <option value="" disabled>Selecciona una categoría</option>
                  <option 
                    v-for="categoria in categorias" 
                    :key="categoria.id" 
                    :value="categoria.id"
                  >
                    {{ categoria.nombre }}
                  </option>
                </select>
              </div>
            </div>

            <div class="form-section">
              <h3 class="section-title">
                <i class="pi pi-list"></i>
                Ingredientes
              </h3>
              
              <div class="ingredientes-container">
                <div class="ingrediente-item" v-for="(ingrediente, index) in ingredientes" :key="index">
                  <div class="ingrediente-content">
                    <select v-model="ingrediente.ingredienteId" class="form-select" style="flex: 2;" required>
                      <option value="" disabled>Selecciona ingrediente</option>
                      <option 
                        v-for="ing in listaIngredientes" 
                        :key="ing.id" 
                        :value="ing.id"
                      >
                        {{ ing.nombre }}
                      </option>
                    </select>
                    <input 
                      v-model="ingrediente.cantidad" 
                      type="number" 
                      min="0.1" 
                      step="0.1"
                      placeholder="Cantidad"
                      class="form-input"
                      style="flex: 1; margin: 0 10px;"
                      required
                    >
                    <select v-model="ingrediente.unidadMedidaId" class="form-select" style="flex: 1;" required>
                      <option value="" disabled>Unidad</option>
                      <option 
                        v-for="unidad in unidadesMedida" 
                        :key="unidad.id" 
                        :value="unidad.id"
                      >
                        {{ unidad.nombre }}
                      </option>
                    </select>
                    <button 
                      type="button" 
                      @click="eliminarIngrediente(index)"
                      class="btn-eliminar"
                    >
                      <i class="pi pi-trash"></i>
                    </button>
                  </div>
                </div>
                
                <button type="button" @click="agregarIngrediente" class="btn-agregar">
                  <i class="pi pi-plus"></i>
                  Agregar Ingrediente
                </button>
              </div>
            </div>

            <div class="form-section">
              <h3 class="section-title">
                <i class="pi pi-directions"></i>
                Pasos de Preparación
              </h3>
              
              <div class="pasos-container">
                <div class="paso-item" v-for="(paso, index) in pasos" :key="index">
                  <div class="paso-header">
                    <span class="paso-numero">Paso {{ index + 1 }}</span>
                    <button 
                      type="button" 
                      @click="eliminarPaso(index)"
                      class="btn-eliminar"
                    >
                      <i class="pi pi-trash"></i>
                    </button>
                  </div>
                  <textarea 
                    v-model="paso.descripcion" 
                    rows="2"
                    placeholder="Describe este paso..."
                    class="form-textarea"
                    required
                  ></textarea>
                </div>
                
                <button type="button" @click="agregarPaso" class="btn-agregar">
                  <i class="pi pi-plus"></i>
                  Agregar Paso
                </button>
              </div>
            </div>

            <div class="form-buttons">
              <button type="button" @click="cancelar" class="btn-cancelar">
                Cancelar
              </button>
              <button type="submit" :disabled="enviando" class="btn-crear">
                <span v-if="enviando">
                  <i class="pi pi-spin pi-spinner"></i>
                  Creando...
                </span>
                <span v-else>
                  <i class="pi pi-check"></i>
                  Crear Receta
                </span>
              </button>
            </div>

            <div v-if="mensajeExito" class="mensaje-exito">
              <i class="pi pi-check-circle"></i>
              {{ mensajeExito }}
            </div>
            
            <div v-if="mensajeError" class="mensaje-error">
              <i class="pi pi-exclamation-triangle"></i>
              {{ mensajeError }}
            </div>

          </form>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import { recetaService } from '../services/recetaService'
import { categoriaService } from '../services/categoriaService'
import { ingredienteService } from '../services/ingredienteService'
import { unidadMedidaService } from '../services/unidadMedidaService'

export default {
  name: 'CrearRecetaView',
  data() {
    return {
      recetaData: {
        titulo: '',
        descripcion: '',
        categoriaId: '',
        tiempoPreparacion: null
      },
      categorias: [],
      listaIngredientes: [], 
      unidadesMedida: [], 
      ingredientes: [], 
      pasos: [], 
      enviando: false,
      mensajeExito: '',
      mensajeError: ''
    }
  },
  async mounted() {
    await this.cargarDatos()
  },
  methods: {
    async cargarDatos() {
      try {
        this.categorias = await categoriaService.obtenerTodas()
        this.listaIngredientes = await ingredienteService.obtenerTodos()  
        this.unidadesMedida = await unidadMedidaService.obtenerTodas()
        this.agregarIngrediente()
        this.agregarPaso()
      } catch (error) {
        console.error('Error cargando datos:', error)
      }
    },

    agregarIngrediente() {
      this.ingredientes.push({
        ingredienteId: '',
        cantidad: '',
        unidadMedidaId: ''
      })
    },

    eliminarIngrediente(index) {
      this.ingredientes.splice(index, 1)
    },

    agregarPaso() {
      this.pasos.push({ descripcion: '' })
    },

    eliminarPaso(index) {
      this.pasos.splice(index, 1)
    },

    async crearReceta() {
      this.enviando = true
      this.mensajeExito = ''
      this.mensajeError = ''
      
      try {
        // Validación frontend mejorada
        const errores = []
        
        if (!this.recetaData.titulo.trim()) {
          errores.push('El título es requerido')
        }
        
        if (!this.recetaData.descripcion.trim()) {
          errores.push('La descripción es requerida')
        }
        
        if (!this.recetaData.categoriaId) {
          errores.push('Debes seleccionar una categoría')
        }
        
        if (!this.recetaData.tiempoPreparacion || this.recetaData.tiempoPreparacion <= 0) {
          errores.push('El tiempo de preparación debe ser mayor a 0')
        }
        
        // Validar ingredientes
        const ingredientesValidos = this.ingredientes.filter(
          i => i.ingredienteId && i.cantidad && i.unidadMedidaId
        )
        
        if (ingredientesValidos.length === 0) {
          errores.push('Debe agregar al menos un ingrediente completo')
        }
        
        // Validar pasos
        const pasosValidos = this.pasos.filter(p => p.descripcion.trim())
        if (pasosValidos.length === 0) {
          errores.push('Debe agregar al menos un paso')
        }
        
        // Si hay errores de frontend, mostrarlos
        if (errores.length > 0) {
          this.mensajeError = errores.join('. ')
          this.enviando = false
          return
        }

        // Preparar datos para enviar
        const datosParaEnviar = {
          titulo: this.recetaData.titulo,
          descripcion: this.recetaData.descripcion,
          tiempoPreparacion: parseInt(this.recetaData.tiempoPreparacion),
          categoriaId: parseInt(this.recetaData.categoriaId),
          pasos: pasosValidos.map(p => p.descripcion).join('\n'),
          ingredientes: ingredientesValidos.map(i => ({
            ingredienteId: parseInt(i.ingredienteId),
            cantidad: parseFloat(i.cantidad),
            unidadMedidaId: parseInt(i.unidadMedidaId)
          }))
        }

        console.log('📡 Enviando datos:', datosParaEnviar)
        
        // Llamar al servicio
        const nuevaReceta = await recetaService.crearReceta(datosParaEnviar)
        
        // Éxito
        this.mensajeExito = `¡Receta "${nuevaReceta.titulo}" creada exitosamente!`
        this.resetFormulario()
        
        setTimeout(() => {
          this.$router.push('/recetas')
        }, 3000)
        
      } catch (error) {
        console.error('❌ Error al crear receta:', error)
        
        // Manejar error del backend
        if (error.response?.data) {
          // Extraer el mensaje del backend
          const errorData = error.response.data
          if (typeof errorData === 'string') {
            this.mensajeError = errorData
          } else if (errorData.title) {
            this.mensajeError = errorData.title
          } else if (errorData.detail) {
            this.mensajeError = errorData.detail
          } else {
            this.mensajeError = 'Error al crear la receta'
          }
        } else {
          this.mensajeError = error.message || 'Error al crear la receta'
        }
      } finally {
        this.enviando = false
      }
    },

    resetFormulario() {
      this.recetaData = {
        titulo: '',
        descripcion: '',
        categoriaId: '',
        tiempoPreparacion: null
      }
      this.ingredientes = [{ ingredienteId: '', cantidad: '', unidadMedidaId: '' }]
      this.pasos = [{ descripcion: '' }]
    },

    cancelar() {
      this.$router.push('/')
    }
  }
}
</script>

<style scoped>
/* css */
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

/* ESTILOS DEL FORMULARIO */
.form-card {
  background: white;
  border-radius: 10px;
  padding: 2rem;
  box-shadow: 0 5px 15px rgba(0,0,0,0.1);
  margin-bottom: 3rem;
}

.form-section {
  margin-bottom: 2.5rem;
  padding-bottom: 2rem;
  border-bottom: 1px solid #eee;
}

.form-section:last-child {
  border-bottom: none;
}

.section-title {
  color: #2c3e50;
  margin-bottom: 1.5rem;
  font-size: 1.3rem;
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 10px;
}

.section-title i {
  color: #e74c3c;
}

.form-row {
  display: flex;
  gap: 1rem;
  margin-bottom: 1rem;
}

.form-row .form-group {
  flex: 1;
}

.form-group {
  margin-bottom: 1.5rem;
}

.form-label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
  color: #2c3e50;
}

.form-input, .form-textarea, .form-select {
  width: 100%;
  padding: 10px 12px;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 1rem;
  font-family: inherit;
  transition: all 0.3s;
}

.form-input:focus, .form-textarea:focus, .form-select:focus {
  outline: none;
  border-color: #3498db;
  box-shadow: 0 0 0 2px rgba(52, 152, 219, 0.2);
}

.form-textarea {
  resize: vertical;
  min-height: 80px;
}

.ingredientes-container {
  margin-top: 1rem;
}

.ingrediente-item {
  background: #f8f9fa;
  border-radius: 6px;
  padding: 1rem;
  margin-bottom: 1rem;
}

.ingrediente-content {
  display: flex;
  gap: 10px;
  align-items: center;
}

.pasos-container {
  margin-top: 1rem;
}

.paso-item {
  background: #f8f9fa;
  border-radius: 6px;
  padding: 1rem;
  margin-bottom: 1rem;
}

.paso-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.5rem;
}

.paso-numero {
  font-weight: 500;
  color: #2c3e50;
}

/* BOTONES */
.btn-eliminar {
  background: #e74c3c;
  color: white;
  border: none;
  width: 36px;
  height: 36px;
  border-radius: 50%;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background 0.3s;
}

.btn-eliminar:hover {
  background: #c0392b;
}

.btn-agregar {
  background: #3498db;
  color: white;
  border: none;
  padding: 10px 16px;
  border-radius: 6px;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  gap: 8px;
  font-size: 0.95rem;
  transition: background 0.3s;
}

.btn-agregar:hover {
  background: #2980b9;
}

.form-buttons {
  display: flex;
  gap: 1rem;
  margin-top: 2rem;
  padding-top: 1.5rem;
  border-top: 1px solid #eee;
}

.btn-crear {
  flex: 2;
  background: #2ecc71;
  color: white;
  border: none;
  padding: 14px;
  border-radius: 6px;
  font-size: 1.1rem;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  transition: background 0.3s;
}

.btn-crear:hover:not(:disabled) {
  background: #27ae60;
}

.btn-crear:disabled {
  background: #95a5a6;
  cursor: not-allowed;
}

.btn-cancelar {
  flex: 1;
  background: #95a5a6;
  color: white;
  border: none;
  padding: 14px;
  border-radius: 6px;
  font-size: 1.1rem;
  cursor: pointer;
  transition: background 0.3s;
}

.btn-cancelar:hover {
  background: #7f8c8d;
}

/* MENSAJES */
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

/* RESPONSIVE */
@media (max-width: 768px) {
  .hero-title {
    font-size: 2.2rem;
  }
  
  .form-row {
    flex-direction: column;
    gap: 0;
  }
  
  .ingrediente-content {
    flex-direction: column;
  }
  
  .ingrediente-content .form-input,
  .ingrediente-content .form-select {
    margin: 5px 0;
    width: 100%;
  }
  
  .form-buttons {
    flex-direction: column;
  }
}
</style>