import api from './api'

export const ingredienteService = {
  async obtenerTodos() {
    try {
      console.log()
      const response = await api.get('/Ingredientes')
      console.log(response.data)
      return response.data
    } catch (error) {
      console.error(error)
      throw error
    }
  }
}