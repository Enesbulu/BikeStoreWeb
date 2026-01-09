import { createContext, useContext, useState, useEffect } from "react";
import api from "../services/api";


const CartContext = createContext();

export const CartProvider = ({ children }) => {
    //Sepeti localeStorage'dan okuacağız sayfa yenilenince silinebilir diye.
    const [cartItems, setCartItems] = useState(() => {
        const saveCart = localStorage.getItem('shoppingCart');
        return saveCart ? JSON.parse(saveCart) : [];
    });

    // Kullanıcı giriş yapmış mı kontrolü (Token var mı?)
    const isLoggedIn = () => !!localStorage.getItem('token');


    //Sepetşn her değiştiğinde localeStorage a kaydediyoruz
    useEffect(() => {
        localStorage.setItem('shoppingCart', JSON.stringify(cartItems));
    }, [cartItems]);

    // Backend'den sepeti çekme fonksiyon
    useEffect(() => {
        const fetchCartFromBackend = async () => {
            // Eğer giriş yapmamışsa hiç backend'e gitme
            if (!isLoggedIn()) return;

            try {
                const response = await api.get('/shoppingcart');

                if (response.data.success && response.data.data) {
                    const backendCart = response.data.data.map(item => ({
                        id: item.productId,
                        name: item.productName,
                        price: item.unitPrice,
                        quantity: item.quantity,
                        imageUrl: item.imageUrl || "https://placehold.co/100"
                    }));

                    if (backendCart.length > 0) {
                        setCartItems(backendCart);
                    }
                }
            } catch (error) {
                console.error("Sepet senkronizasyon hatası:", error);
            }
        };

        fetchCartFromBackend();
    }, []); // [] sayesinde sadece sayfa ilk açıldığında 1 kere çalışır.


    //Sepete ürün ekleme
    const addToCart = (product) => {
        //Sepet elemanlarını güncelle
        setCartItems((prevItems) => {
            //Ürün zaten sepette varsa kontrolü
            const existingItem = prevItems.find((item) => item.id === product.id);
            if (existingItem) {
                //varsa miktarını(quantity) 1 arttır
                return prevItems.map((item) => item.id === product.id ? { ...item, quantity: item.quantity + 1 } : item);
            } else {
                //yoksa yeni ürün olarak ekle 1 adet olarak
                return [...prevItems, { ...product, quantity: 1 }];
            }
        });
        // Eğer kullanıcı giriş yapmışsa backend'e de ekle
        if (isLoggedIn()) {
            // Backend'e ekleme işlemi
            const payload = {
                productId: product.id,
                quantity: 1
            };
            api.post('/shoppingcart/add', payload)
                .then(() => console.log("Ürün DB sepetine işlendi."))
                .catch((err) => console.error("Ürün DB sepetine eklenirken hata oluştu:", err));

        };
        alert("Ürün sepete eklendi!");
    };

    //sepetten ürün silme
    const removeFromCart = (productId) => {
        setCartItems((prevItems) => prevItems.filter((item) => item.id !== productId));

        if (isLoggedIn()) {
            // Backend'den silme işlemi
            api.delete(`/shoppingcart/remove/${productId}`)
                .then(() => console.log("Ürün DB sepetinden silindi."))
                .catch((err) => console.error("Ürün DB sepetinden silinirken hata oluştu:", err));
        }
    };

    //sepeti temizleme
    const clearCart = () => {
        setCartItems([]);
        if (isLoggedIn()) {
            // Backend'den sepeti temizleme işlemi
            api.delete('/shoppingcart/clear')
                .then(() => console.log("DB sepeti temizlendi."))
                .catch((err) => console.error("DB sepeti temizlenirken hata oluştu:", err));
        }
    };


    //toplam sepet tutarı
    const cartTotal = cartItems.reduce((total, item) => total + (item.price * item.quantity), 0);

    //sepetteki toplam ürün sayısı
    const cartCount = cartItems.reduce((count, item) => count + item.quantity, 0);

    return (
        <CartContext.Provider value={{ cartItems, addToCart, removeFromCart, clearCart, cartTotal, cartCount }}> {children}</CartContext.Provider>
    );
};


//kullanım kolaylığı için özel hook tanımalaması yapıyorum
// eslint-disable-next-line react-refresh/only-export-components
export const useCart = () => useContext(CartContext);   