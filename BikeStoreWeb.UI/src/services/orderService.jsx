import api from './api';

const orderService = {
    orderService: (chechoutDto) => {
        return api.post('/orders/checkout', chechoutDto);
    },


    getMyOrders: () => {
        return api.get('/orders/customer/orders');
    }
};

export default orderService;