import api from './api'

export const categoriaService = {
  async obtenerTodas() {
    try {
      console.log()
      const response = await api.get('/Categorias')
      console.log(response.data)
      return response.data
    } catch (error) {
      console.error( error)
      throw error
    }
  },
  
  async obtenerPorId(id) {
    try {
      const response = await api.get(`/Categorias/${id}`)
      return response.data
    } catch (error) {
      console.error(`${id}:`, error)
      throw error
    }
  },
   async obtenerCategoriasMasUsadas() {
    try {
      const response = await api.get('/Categorias/mas-usadas')
      return response.data
    } catch (error) {
      console.error('Error obteniendo categorías más usadas:', error)
      throw error
    }
  }
}