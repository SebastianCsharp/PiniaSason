<template>
  <div class="page-container">
    <header class="page-hero">
      <div class="hero-overlay">
        <div class="hero-content">
          <h1 class="hero-title">Editar Receta</h1>
          <p class="hero-subtitle">Actualiza los datos de tu receta</p>
        </div>
      </div>
    </header>

    <section class="content-section">
      <div class="container">
        <div class="filters-bar">
          <button @click="cancelar" class="btn-inicio">
            <i class="pi pi-arrow-left"></i>
            Cancelar
          </button>
          <div class="page-info">
            <span class="recipe-id">ID: {{ recetaId }}</span>
          </div>
        </div>

        <div v-if="cargando" class="loading-state">
          <i class="pi pi-spin pi-spinner"></i>
          Cargando receta...
        </div>

        <div v-else class="form-card">
          <form @submit.prevent="actualizarReceta">
            <div class="form-section">
              <h3 class="section-title">
                <i class="pi pi-info-circle"></i>
                Información Básica
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

            <!-- ingredi -->
            <div class="form-section">
              <h3 class="section-title">
                <i class="pi pi-list"></i>
                Ingredientes
              </h3>
              
              <div class="ingredientes-container">
                <div class="ingrediente-item" v-for="(ingrediente, index) in ingredientes" :key="index">
                  <div class="ingrediente-content">
                    <select v-model="ingrediente.ingredienteId" class="form-select" style="flex: 2;">
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
                    >
                    <select v-model="ingrediente.unidadMedidaId" class="form-select" style="flex: 1;">
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

            <!-- pasos -->
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
                  ></textarea>
                </div>
                
                <button type="button" @click="agregarPaso" class="btn-agregar">
                  <i class="pi pi-plus"></i>
                  Agregar Paso
                </button>
              </div>
            </div>

            <!-- botones -->
            <div class="form-buttons">
              <button type="button" @click="cancelar" class="btn-cancelar">
                Cancelar
              </button>
              <button type="submit" :disabled="enviando" class="btn-actualizar">
                <span v-if="enviando">
                  <i class="pi pi-spin pi-spinner"></i>
                  Actualizando...
                </span>
                <span v-else>
                  <i class="pi pi-check"></i>
                  Actualizar Receta
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
  name: 'EditarRecetaView',
  props: {
    id: {
      type: [String, Number],
      required: true
    }
  },
  data() {
    return {
      recetaId: null,
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
      cargando: true,
      enviando: false,
      mensajeExito: '',
      mensajeError: ''
    }
  },
  computed: {
    recetaIdNumber() {
      return Number(this.id)
    }
  },
  async mounted() {
    this.recetaId = this.recetaIdNumber
    await this.cargarDatos()
  },
  methods: {
    async cargarDatos() {
      try {
        this.cargando = true
        await Promise.all([
          this.cargarCategorias(),
          this.cargarIngredientes(),
          this.cargarUnidadesMedida()
        ])
        //carga receta exis
        await this.cargarReceta()
        
      } catch (error) {
        console.error('Error cargando datos:', error)
        this.mensajeError = 'Error al cargar los datos de la receta'
      } finally {
        this.cargando = false
      }
    },
    
    async cargarCategorias() {
      try {
        this.categorias = await categoriaService.obtenerTodas()
      } catch (error) {
        console.error('Error cargando categorías:', error)
        this.categorias = []
      }
    },
    
    async cargarIngredientes() {
      try {
        this.listaIngredientes = await ingredienteService.obtenerTodos()
      } catch (error) {
        console.error('Error cargando ingredientes:', error)
        this.listaIngredientes = []
      }
    },
    
    async cargarUnidadesMedida() {
      try {
        this.unidadesMedida = await unidadMedidaService.obtenerTodas()
      } catch (error) {
        console.error('Error cargando unidades:', error)
        this.unidadesMedida = []
      }
    },
    
    async cargarReceta() {
      try {
        const receta = await recetaService.obtenerPorId(this.recetaId)
        
        if (!receta) {
          this.mensajeError = 'Receta no encontrada'
          return
        }
        //cargadatos
        this.recetaData = {
          titulo: receta.titulo,
          descripcion: receta.descripcion || '',
          categoriaId: this.obtenerCategoriaId(receta.categoriaNombre),
          tiempoPreparacion: receta.tiempoPreparacion
        }
        //carga ingre
        if (receta.ingredientes && receta.ingredientes.length > 0) {
          this.ingredientes = receta.ingredientes.map(ing => ({
            ingredienteId: this.obtenerIngredienteId(ing.nombre),
            cantidad: ing.cantidad,
            unidadMedidaId: this.obtenerUnidadMedidaId(ing.unidadMedida)
          }))
        } else {
          this.agregarIngrediente()
        }
        if (receta.pasos) {
          const pasosArray = receta.pasos.split('\n').filter(p => p.trim())
          this.pasos = pasosArray.map(p => ({ descripcion: p.trim() }))
        }
        
        if (this.pasos.length === 0) {
          this.agregarPaso()
        }
        
      } catch (error) {
        console.error('Error cargando receta:', error)
        this.mensajeError = 'Error al cargar la receta'
      }
    },
    
    obtenerCategoriaId(nombreCategoria) {
      const categoria = this.categorias.find(c => c.nombre === nombreCategoria)
      return categoria ? categoria.id : ''
    },
    
    obtenerIngredienteId(nombreIngrediente) {
      const ingrediente = this.listaIngredientes.find(i => i.nombre === nombreIngrediente)
      return ingrediente ? ingrediente.id : ''
    },
    
    obtenerUnidadMedidaId(abreviatura) {
      const unidad = this.unidadesMedida.find(u => u.abreviatura === abreviatura)
      return unidad ? unidad.id : ''
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
    
    async actualizarReceta() {
      this.enviando = true
      this.mensajeExito = ''
      this.mensajeError = ''
      
      try {
        if (!this.recetaData.titulo.trim()) {
          throw new Error('El título es requerido')
        }
        if (!this.recetaData.categoriaId) {
          throw new Error('Debes seleccionar una categoría')
        }
        if (!this.recetaData.tiempoPreparacion || this.recetaData.tiempoPreparacion <= 0) {
          throw new Error('El tiempo de preparación debe ser mayor a 0')
        }
         //prepara datos pa enviar
        const datosParaEnviar = {
          titulo: this.recetaData.titulo,
          descripcion: this.recetaData.descripcion,
          tiempoPreparacion: Number(this.recetaData.tiempoPreparacion),
          categoriaId: Number(this.recetaData.categoriaId),
          pasos: this.pasos
            .filter(p => p.descripcion.trim())
            .map(p => p.descripcion)
            .join('\n'),
          ingredientes: this.ingredientes
            .filter(i => i.ingredienteId && i.cantidad && i.unidadMedidaId)
            .map(i => {
              const ingredienteSeleccionado = this.listaIngredientes.find(
                ing => ing.id === parseInt(i.ingredienteId)
              )
              
              return {
                ingredienteId: parseInt(i.ingredienteId),
                nombre: ingredienteSeleccionado ? ingredienteSeleccionado.nombre : '',
                cantidad: parseFloat(i.cantidad),
                unidadMedidaId: parseInt(i.unidadMedidaId)
              }
            })
        }

        console.log('📤 Enviando datos de actualización:', datosParaEnviar)
        const recetaActualizada = await recetaService.actualizarReceta(this.recetaId, datosParaEnviar)
        this.mensajeExito = `¡Receta "${recetaActualizada.titulo}" actualizada exitosamente!`
        // redirigir después de 2 segundos
        setTimeout(() => {
          this.$router.push('/recetas/gestionar')
        }, 2000)
        
      } catch (error) {
        console.error('❌ Error actualizando receta:', error)
        this.mensajeError = error.response?.data?.title || error.message || 'Error al actualizar la receta'
      } finally {
        this.enviando = false
      }
    },
    
    cancelar() {
      this.$router.push('/recetas/gestionar')
    }
  }
}
</script>

<style scoped>
/* estilo base */
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

.page-info {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.recipe-id {
  background: #3498db;
  color: white;
  padding: 8px 16px;
  border-radius: 20px;
  font-weight: 500;
}

/* estilo de formulario */
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

/* ingre */
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

/* pasos */
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

/* botones */
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

.btn-actualizar {
  flex: 2;
  background: #f39c12;
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

.btn-actualizar:hover:not(:disabled) {
  background: #e67e22;
}

.btn-actualizar:disabled {
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

/* mensajes */
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

.loading-state {
  text-align: center;
  padding: 3rem;
  color: #7f8c8d;
}

.loading-state i {
  font-size: 2rem;
  margin-bottom: 1rem;
}
@media (max-width: 768px) {
  .hero-title {
    font-size: 2.2rem;
  }
  
  .filters-bar {
    flex-direction: column;
    gap: 1rem;
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