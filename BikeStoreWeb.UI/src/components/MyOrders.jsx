import { useEffect, useState } from "react";
import orderService from "../services/orderService";

const MyOrders = () => {
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchOrders = async () => {
            try {
                const response = await orderService.getMyOrders();
                setOrders(response.data);
            } catch (error) {
                console.error("Error fetching orders:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchOrders();
    }, []);
    if (loading) {
        return <div>Yükleniyor...</div>;
    }
    return (
        <div className="container mt-4">
            <h2 className="mb-4">Geçmiş Siparişlerim</h2>

            {orders.length === 0 ? (
                <div className="alert alert-info">Henüz hiç sipariş vermediniz.</div>
            ) : (
                <div className="accordion" id="ordersAccordion">
                    {orders.data.map((order, index) => (
                        <div className="card mb-3" key={order.id || index}>
                            {/* Sipariş Başlığı (Header) */}
                            <div className="card-header d-flex justify-content-between align-items-center">
                                <div>
                                    <strong>Sipariş No:</strong> {order.orderNumber} <br />
                                    <small className="text-muted">
                                        {new Date(order.orderDate).toLocaleDateString('tr-TR')}
                                        {' - '}
                                        {new Date(order.orderDate).toLocaleTimeString('tr-TR', { hour: '2-digit', minute: '2-digit' })}
                                    </small>
                                </div>
                                <div className="text-end">
                                    <span className={`badge ${order.status === 'Tamamlandı' ? 'bg-success' : 'bg-warning text-dark'}`}>
                                        {order.status}
                                    </span>
                                    <div className="fw-bold mt-1">
                                        {order.totalAmount} ₺
                                    </div>
                                </div>
                            </div>

                            {/* Sipariş Detayları (Body) - Basit Listeleme */}
                            <div className="card-body">
                                <h6 className="card-subtitle mb-2 text-muted">Ürünler:</h6>
                                <ul className="list-group list-group-flush">
                                    {order.items && order.items.map((item, i) => (
                                        <li key={i} className="list-group-item d-flex justify-content-between align-items-center">
                                            <div>
                                                {/* Backend DTO'da ProductName varsa onu yaz, yoksa ID yaz */}
                                                {item.productName ? item.productName : `Ürün ID: ${item.productId}`}
                                                <span className="badge bg-secondary ms-2">x{item.quantity}</span>
                                            </div>
                                            <span>{item.price * item.quantity} ₺</span>
                                        </li>
                                    ))}
                                </ul>
                                <div className="mt-2 text-end">
                                    <small>Teslimat Adresi: {order.shippingAddress || "Adres belirtilmedi"}</small>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}


export default MyOrders;