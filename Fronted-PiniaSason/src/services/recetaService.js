import api from './api'

export const recetaService = {
  async obtenerTodas() {
    try {
      console.log()
      const response = await api.get('/recetas')
      console.log(response)
      return response.data
    } catch (error) {
      console.error(error)
      console.error(error.response)
      throw error
    }
  },
  
 async obtenerPorCategoria(categoriaNombre) {
  try {
    const categoriaCodificada = encodeURIComponent(categoriaNombre)
    const response = await api.get(`/recetas/categoria/${categoriaCodificada}`)
    return response.data
  } catch (error) {
    console.error(`Error obteniendo recetas por categoría "${categoriaNombre}":`, error)
    throw error
  }
  },

  async obtenerPorId(id) {
    try {
      console.log(`${id}`)
      const response = await api.get(`/recetas/${id}`)
      console.log(response.data)
      return response.data
    } catch (error) {
      console.error(`${id}:`, error)
      console.error(error.response)
      throw error
    }
  },
  
async crearReceta(recetaData) {
    try {
      console.log(recetaData)
      const response = await api.post('/recetas', recetaData)
      console.log('Receta creada exitosamente:', response.data)
      return response.data
    } catch (error) {
      console.error(error)
      
      // Verificar si es error de validación (400)
      if (error.response?.status === 400) {
        const errorMessage = error.response?.data?.title || error.response?.data || error.message;
        alert(`Error de validación: ${errorMessage}`);
      } else {
        // Para otros errores (500, etc.)
        alert(`Error al crear receta: ${error.message}`);
      }
      
      throw error
    }
},
async actualizarReceta(id, recetaData) {
  try {
    console.log(`📡 Actualizando receta ${id}...`, recetaData)
    const response = await api.put(`/Recetas/${id}`, recetaData)
    console.log('✅ Receta actualizada:', response.data)
    return response.data
  } catch (error) {
    console.error(`❌Error actualizando receta ${id}:`, error)
    throw error
  }
},
async eliminarReceta(id) {
  try {
    console.log(`Eliminando receta ${id}...`)
    const response = await api.delete(`/Recetas/${id}`)
    console.log('🗑️ Receta eliminada')
    return response.data
  } catch (error) {
    console.error(`${id}:`, error)
    throw error
  }
},
async obtenerDestacadas() {
  try {
    console.log()
    const response = await api.get('/Recetas/destacadas')
    console.log(response.data)
    return response.data
  } catch (error) {
    console.error(error)
    throw error
  }
},
}