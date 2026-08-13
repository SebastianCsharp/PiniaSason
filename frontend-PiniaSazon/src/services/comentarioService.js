import api from './api'

export const comentarioService = {
  async obtenerPorReceta(recetaId) {
    try {
      console.log(recetaId)
      const response = await api.get(`/recetas/${recetaId}/comentarios`)
      console.log(response.data)
      return response.data
    } catch (error) {
      console.error(error)
      if (error.response?.status === 404) {
        return []
      }
      throw error
    }
  },

  async crear(recetaId, comentarioData) {
    try {
      console.log(comentarioData)
      const response = await api.post(`/recetas/${recetaId}/comentarios`, comentarioData)
      console.log(response.data)
      return response.data
    } catch (error) {
      console.error(error)
      throw error
    }
  },

  async actualizar(recetaId, comentarioId, datos) {
    try {
      console.log('Actualizando comentario:', datos)
      const response = await api.put(`/recetas/${recetaId}/comentarios/${comentarioId}`, datos)
      console.log('Comentario actualizado:', response.data)
      return response.data
    } catch (error) {
      console.error(error)
      throw error
    }
  },

  async eliminar(recetaId, comentarioId) {
    try {
      console.log(comentarioId)
      await api.delete(`/recetas/${recetaId}/comentarios/${comentarioId}`)
      console.log('🗑️Comentario eliminado')
    } catch (error) {
      console.error(error)
      throw error
    }
  }
}