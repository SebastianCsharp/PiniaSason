import api from './api'

export const unidadMedidaService = {
  async obtenerTodas() {
    try {
      console.log()
      const response = await api.get('/UnidadesMedida')
      console.log(response.data)
      return response.data
    } catch (error) {
      console.error(error)
      throw error
    }
  }
}