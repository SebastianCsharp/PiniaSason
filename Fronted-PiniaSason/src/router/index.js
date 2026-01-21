import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import RecetasView from '../views/RecetasView.vue'
import RecetaDetalleView from '../views/RecetaDetalleView.vue' 

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/recetas',
    name: 'recetas',
    component: RecetasView
  },
  {
    path: '/receta/:id', 
    name: 'receta-detalle',
    component: RecetaDetalleView
  },
  {
  path: '/recetas/categorias',
  name: 'RecetasPorCategoria',
  component: () => import('../views/RecetasPorCategoriaView.vue')
  },
  {
  path: '/recetas/crear',
  name: 'CrearReceta',
  component: () => import('../views/CrearRecetaView.vue')
  },
  {
  path: '/recetas/gestionar',
  name: 'GestionarRecetas',
  component: () => import('../views/GestionarRecetasView.vue')
  },
  {
  path: '/recetas/editar/:id',
  name: 'EditarReceta',
  component: () => import('../views/EditarRecetaView.vue'),
  props: true
  },
  {
  path: '/recetas/destacadas',
  name: 'RecetasDestacadas',
  component: () => import('../views/RecetasDestacadasView.vue')
  },
   {
    path: '/categorias-especiales',
    name: 'CategoriasEspeciales',
    component: () => import ('../views/CategoriasEspecialesView.vue')
  }

]
const router = createRouter({
  history: createWebHistory(),
  routes
})
export default router
