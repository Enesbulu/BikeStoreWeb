import { createContext, useContext, useState, useEffect } from "react";


const CartContext = createContext();

export const CartProvider = ({ children }) => {
    //Sepeti localeStorage'dan okuacağız sayfa yenilenince silinebilir diye.
    const [cartItems, setCartItems] = useState(() => {
        const saveCart = localStorage.getItem('shoppingCart');

        return saveCart ? JSON.parse(saveCart) : [];
    });

    //Sepetşn her değiştiğinde localeStorage a kaydediyoruz
    useEffect(() => {
        localStorage.setItem('shoppingCart', JSON.stringify(cartItems));
    }, [cartItems]);

    //Sepete ürün ekleme
    const addToCart = (product) => {
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
        alert("Ürün sepete eklendi!");
    };

    //sepetten ürün silme
    const removeFromCart = (id) => {
        setCartItems((prevItems) => prevItems.filter((item) => item.id !== id));
    };

    //sepeti temizleme
    const clearCart = () => {
        setCartItems([]);
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